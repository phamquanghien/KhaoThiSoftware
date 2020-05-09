using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using KhaoThiSoftware.Models;
using KhaoThiSoftware.Reports.rpt;
using System.Data;
using System.IO;
using CrystalDecisions.Shared;

namespace KhaoThiSoftware.Areas.Admins.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KyThi_AdController : Controller
    {
        private KhaoThiDBContext db = new KhaoThiDBContext();

        // GET: Admins/KyThi_Ad
        public ActionResult Index()
        {
            return View(db.KyThis.Where(m => m.isDelete == false).ToList());
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
                return View("~/Views/Shared/404Notfound.cshtml");
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
            if (kyThi == null || kyThi.isDelete == true)
            {
                return View("~/Views/Shared/404Notfound.cshtml");
            }
            var check = db.DanhSachThis.Where(m => m.IdKyThi == id).Count();
            if (check > 0)
            {
                ViewBag.ThongBao = "Kỳ thi đang có dữ liệu nếu xóa kỳ thi sẽ xóa toàn bộ dữ liệu liên quan. Bạn có muốn xóa kỳ thi không?";
            }
            else
            {
                ViewBag.ThongBao = "Bạn có muốn xóa kỳ thi không?";
            }
            return View(kyThi);
        }

        // POST: Admins/KyThi_Ad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var check = db.DanhSachThis.Where(m => m.IdKyThi == id).Count();
            KyThi kyThi = db.KyThis.Find(id);
            if (check > 0)
            {
                kyThi.isDelete = true;
                db.SaveChanges();
            }
            else
            {
                db.KyThis.Remove(kyThi);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult ThongKePhach(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KyThi kyThi = db.KyThis.Find(id);
            if (kyThi == null || kyThi.isDelete == true)
            {
                return View("~/Views/Shared/404Notfound.cshtml");
            }
            var model = (from tkp in db.ThongKePhachs
                         join kt in db.KyThis
                         on tkp.IdKyThi equals kt.IdKyThi
                         where tkp.IdKyThi == id
                         
                         select new {
                             tenKyThi = kt.TenKyThi,
                             tenMonThi = tkp.TenMonThi,
                             phachBD = tkp.PhachBatDau,
                             phachKT = tkp.PhachKetThuc

                         }).Distinct().ToList();
            rptThongKePhach rpt = new rptThongKePhach();
            DataTable dt = new DataTable();
            dt.Columns.Add("TenKyThi", typeof(String));
            dt.Columns.Add("TenMonThi", typeof(String));
            dt.Columns.Add("PhachBatDau", typeof(String));
            dt.Columns.Add("PhachKetThuc", typeof(String));
            for (int i = 0; i < model.Count; i++)
            {
                dt.Rows.Add(model[i].tenKyThi, model[i].tenMonThi, model[i].phachBD, model[i].phachKT);
            }
            rpt.Load();
            rpt.SetDataSource(dt);
            Stream s = rpt.ExportToStream(ExportFormatType.WordForWindows);
            return File(s, "application/docx", "Report.doc");
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
