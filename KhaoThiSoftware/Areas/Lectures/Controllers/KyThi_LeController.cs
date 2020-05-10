using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KhaoThiSoftware.Models;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace KhaoThiSoftware.Areas.Lectures.Controllers
{
    [Authorize(Roles = "Lecture")]
    public class KyThi_LeController : Controller
    {
        KhaoThiDBContext db = new KhaoThiDBContext();
        ExcelProcess excelPro = new ExcelProcess();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["KhaoThiDBContext"].ConnectionString);
        public ActionResult Index()
        {
            using (var db = new KhaoThiDBContext())
            {
                var model = db.KyThis.Where(m => m.isDelete == false).ToList();
                return View(model);
            }
        }
        public ActionResult CheckPhach(int? id)
        {
            if (id == null) return RedirectToAction("Index", "KyThi_Le");
            var checkKyThi = db.KyThis.Where(m => m.IdKyThi == id && m.isDelete == false).Count();
            if (checkKyThi < 1)
            {
                return View("~/Views/Shared/404Notfound.cshtml");
            }
            ViewBag.tenKyThi = db.KyThis.Where(m => m.IdKyThi == id && m.isDelete == false).First().TenKyThi;
            ViewBag.idKyThi = id;
            //var model = db.DanhSachThis.Where(m => m.IdKyThi == id).Select(m => m.f_tenmhvn).Distinct().ToList();
            var model = db.DanhSachThis.Where(m => m.IdKyThi == id).Select(m => m.f_tenmhvn).Distinct().ToList();
            model.Sort();
            return View(model);
        }
        [HttpPost]
        public ActionResult CheckPhach(HttpPostedFileBase file, int? idKyThi, string tenMonHoc)
        {
            //try
            {
                if (file.ContentLength > 0)
                {
                    CopyDataFormExcel(file, idKyThi, tenMonHoc);
                }
                //list phach khop dung
                var listDiem = (from tnd in db.TestNhapDiems
                                    join dsp in db.DanhSachPhachs
                                    on tnd.SoPhach equals dsp.SoPhach
                                    join dst in db.DanhSachThis
                                    on dsp.IdDanhSachThi equals dst.IdDanhSachThi
                                    where tnd.IdKyThi == idKyThi && dsp.IdKyThi == idKyThi && dst.f_tenmhvn == tenMonHoc
                                    select new
                                    {
                                        soPhach = tnd.SoPhach
                                    }).ToList();
                //list phach
                var listPhach = (from dsp in db.DanhSachPhachs
                                join dst in db.DanhSachThis
                                on dsp.IdDanhSachThi equals dst.IdDanhSachThi
                                where dsp.IdKyThi == idKyThi && dst.f_tenmhvn == tenMonHoc
                                select new
                                {
                                    soPhach = dsp.SoPhach
                                }).ToList();
                return View("");
            }
            //catch
            //{
            //    return Json("Định dạng file không chính xác. Vui lòng kiểm tra lại (*.xls)", JsonRequestBehavior.AllowGet);
            //}
        }
        
        private void CopyDataFormExcel(HttpPostedFileBase file, int? idKyThi, string tenMonHoc)
        {
            var maKyThi = db.KyThis.Find(idKyThi).MaKyThi;
            var dtNow = DateTime.Now;
            string _FileName = "Le-" + maKyThi + "-" + dtNow.Year + dtNow.Month + dtNow.Day + dtNow.Hour + dtNow.Minute + dtNow.Second + ".xls";
            string _path = Path.Combine(Server.MapPath("~/Uploads/Excels"), _FileName);
            file.SaveAs(_path);
            DataTable dt = excelPro.ReadDataFromExcelFile(_path);
            dt.Columns.Add("IdKyThi", typeof(int));
            dt.Columns.Add("Active", typeof(bool));
            dt.Columns.Add("f_tenmhvn", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][4] = idKyThi;
                dt.Rows[i][5] = false;
                dt.Rows[i][6] = tenMonHoc;
            }

            SqlBulkCopy bulkcopy = new SqlBulkCopy(con);
            bulkcopy.DestinationTableName = "TestNhapDiems";
            bulkcopy.ColumnMappings.Add(0, "SoPhach");
            bulkcopy.ColumnMappings.Add(1, "Diem1");
            bulkcopy.ColumnMappings.Add(2, "Diem2");
            bulkcopy.ColumnMappings.Add(3, "DiemTrungBinh");
            bulkcopy.ColumnMappings.Add(4, "IdKyThi");
            bulkcopy.ColumnMappings.Add(5, "Active");
            bulkcopy.ColumnMappings.Add(5, "f_tenmhvn");
            con.Open();
            bulkcopy.WriteToServer(dt);
            con.Close();
        }
    }
}