using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using KhaoThiSoftware.Models;

namespace KhaoThiSoftware.Areas.Admins.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KyThi_AdController : Controller
    {
        private KhaoThiDBContext db = new KhaoThiDBContext();

        // GET: Admins/KyThi_Ad
        public ActionResult Index()
        {
            return View(db.KyThis.ToList());
        }
        
        // GET: Admins/KyThi_Ad/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admins/KyThi_Ad/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdKyThi,MaKyThi,TenKyThi,NgayTao,NguoiTao,ChuThich,isDelete,Status")] KyThi kyThi)
        {
            kyThi.NgayTao = DateTime.Now;
            kyThi.isDelete = false;
            kyThi.Status = true;
            kyThi.NguoiTao = Session["idUser"].ToString();
            if (ModelState.IsValid)
            {
                db.KyThis.Add(kyThi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kyThi);
        }

        // GET: Admins/KyThi_Ad/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KyThi kyThi = db.KyThis.Find(id);
            if (kyThi == null)
            {
                return HttpNotFound();
            }
            return View(kyThi);
        }

        // POST: Admins/KyThi_Ad/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdKyThi,MaKyThi,TenKyThi,NgayTao,NguoiTao,ChuThich,isDelete,Status")] KyThi kyThi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kyThi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kyThi);
        }

        // GET: Admins/KyThi_Ad/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KyThi kyThi = db.KyThis.Find(id);
            if (kyThi == null)
            {
                return HttpNotFound();
            }
            return View(kyThi);
        }

        // POST: Admins/KyThi_Ad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KyThi kyThi = db.KyThis.Find(id);
            db.KyThis.Remove(kyThi);
            db.SaveChanges();
            return RedirectToAction("Index");
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
