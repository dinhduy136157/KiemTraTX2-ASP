using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using KiemTraTX2_ASP.Models;

namespace KiemTraTX2_ASP.Controllers
{
    public class HocSinhController : Controller
    {
        private readonly DbContext _context;

        private HSEntities db = new HSEntities();

        // GET: HocSinh
        public ActionResult XemDanhSach(string searchString)
        {
            var hocSinhs = from h in db.HocSinhs
                           select h;

            if (!String.IsNullOrEmpty(searchString))
            {
                hocSinhs = hocSinhs.Where(h => h.hoten.Contains(searchString) || h.sbd.Contains(searchString));
            }

            ViewBag.CurrentFilter = searchString;
            return View(hocSinhs.ToList());
        }


        // GET: HocSinh/Create
        public ActionResult ThemDuLieu()
        {
            ViewBag.malop = new SelectList(db.LopHocs, "malop", "tenlop");
            return View();
        }

        // POST: HocSinh/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemDuLieu([Bind(Include = "sbd,hoten,anhduthi,malop,diemthi")] HocSinh hocSinh)
        {
            if (ModelState.IsValid)
            {

                var f = Request.Files["FileName"];
                if (f != null && f.ContentLength > 0)
                {
                    string tenfile = System.IO.Path.GetFileName(f.FileName);
                    string duongdan = Server.MapPath("~/Images/" + tenfile);
                    f.SaveAs(duongdan);
                    hocSinh.anhduthi = tenfile;
                }

                ViewBag.malop = new SelectList(db.LopHocs, "malop", "tenlop", hocSinh.malop);

                var h = db.HocSinhs.FirstOrDefault(x => x.sbd == hocSinh.sbd);
                if (h != null)
                {
                    ViewBag.msg = "ton tai ma hoc sinh";
                    return View();
                }


                db.HocSinhs.Add(hocSinh);
                db.SaveChanges();
                return RedirectToAction("/XemDanhSach");
            }

            return View(hocSinh);
        }

        // GET: HocSinh/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocSinh hocSinh = db.HocSinhs.Find(id);
            if (hocSinh == null)
            {
                return HttpNotFound();
            }
            return View(hocSinh);
        }

        // POST: HocSinh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sbd,hoten,anhduthi,malop,diemthi")] HocSinh hocSinh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hocSinh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("/XemDanhSach");
            }
            return View(hocSinh);
        }

        // GET: HocSinh/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocSinh hocSinh = db.HocSinhs.Find(id);
            if (hocSinh == null)
            {
                return HttpNotFound();
            }
            return View(hocSinh);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HocSinh hocSinh = db.HocSinhs.Find(id);
            if (hocSinh == null)
            {
                TempData["DeleteMessage"] = "Delete failed. Record not found.";
            }
            else
            {
                db.HocSinhs.Remove(hocSinh);
                db.SaveChanges();
                TempData["DeleteMessage"] = "Delete successful.";
            }
            return RedirectToAction("XemDanhSach");
        }





        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
