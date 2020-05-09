using System;
using System.Linq;
using System.Web.Mvc;
using KhaoThiSoftware.Models;
using System.Data;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using KhaoThiSoftware.Reports.rpt;
using CrystalDecisions.Shared;

namespace KhaoThiSoftware.Areas.Admins.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DanhSachThi_AdController : Controller
    {
        KhaoThiDBContext db = new KhaoThiDBContext();
        ExcelProcess excelPro = new ExcelProcess();
        ProcessLogic proLogic = new ProcessLogic();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["KhaoThiDBContext"].ConnectionString);
        
        // GET: Admins/DanhSachThi_Ad
        public ActionResult Index(int? id)
        {
            if (id == null) return RedirectToAction("Index", "KyThi_Ad");
            var checkKyThi = db.KyThis.Where(m => m.IdKyThi == id && m.isDelete == false).Count();
            if (checkKyThi < 1)
            {
                return View("~/Views/Shared/404Notfound.cshtml");
            }
            var model = db.DanhSachThis.Where(m => m.IdKyThi == id).ToList();
            ViewBag.soThiSinh = model.Where(m => m.sobaodanh!=0).ToList().Count;
            ViewBag.soMonThi = model.Where(m => m.sobaodanh == 0).ToList().Count;
            ViewBag.idkt = id;
            ViewBag.tenKyThi = db.KyThis.Where(m => m.IdKyThi == id).First().TenKyThi;
            return View();
        }
        //xoa du lieu di nhap lai
        [HttpPost]
        public ActionResult OverwriteData(HttpPostedFileBase file, int? idKyThi)
        {
            // delete data from Danh sach thi
            db.DanhSachThis.RemoveRange(db.DanhSachThis.Where(m => m.IdKyThi == idKyThi));
            db.SaveChanges();
            // delete data from Danh sach phach
            db.DanhSachPhachs.RemoveRange(db.DanhSachPhachs.Where(m => m.IdKyThi == idKyThi));
            db.SaveChanges();

            try
            {
                if (file.ContentLength > 0)
                {
                    CopyDataFormExcel(file, idKyThi);
                    InsertDataMonHoc(idKyThi);
                }

                return RedirectToAction("Index", "DanhSachThi_Ad", new { id = idKyThi });
            }
            catch
            {
                return View("UpLoadFail");
            }
        }
        //nhap du lieu bo sung
        [HttpPost]
        public ActionResult AppendData(HttpPostedFileBase file, int? idKyThi)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    CopyDataFormExcel(file, idKyThi);
                    //Neu chua co thong tin mon thi them vao
                    if (db.DanhSachThis.Where(m => m.sobaodanh == 0 && m.IdKyThi == idKyThi).Count() == 0)
                    {
                        InsertDataMonHoc(idKyThi);
                    }
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return RedirectToAction("Index", "DanhSachThi_Ad", new { id = idKyThi });
            }
            catch
            {
                return View("UpLoadFail");
            }
        }

        public JsonResult GetData(int id)
        {
            var model = db.DanhSachThis.Where(m => m.IdKyThi == id).Take(1000).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFileMau()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/Excels/";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + "FileMau.xls");
            string fileName = "FileMau.xls";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult GeneratePhach(int? idkt)
        {
            if (idkt == null)
            {
                return Json("Sinh phách thất bại. Lý do: Chưa chọn kỳ thi", JsonRequestBehavior.AllowGet);
            }
            var listDanhSachThi = db.DanhSachThis.Where(m => m.IdKyThi == idkt).ToList();
            if (listDanhSachThi.Count() == 0)
            {
                return Json("Sinh phách thất bại. Lý do: Chưa có danh sách thí sinh của kỳ thi", JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (db.DanhSachPhachs.Where(m => m.IdKyThi == idkt).Count() == 0)
                {
                    var countPhach = InsertDataPhach(idkt);
                    return Json("Sinh thành công " + countPhach + " phách!", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int countPhachBoSung = InsertDataPhachBoSung(idkt);
                    //sinh them phach neu import them danh sach thi sinh
                    if (countPhachBoSung > 0)
                    {
                        return Json("Sinh bổ sung thành công " + countPhachBoSung + " phách!", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Sinh phách của kỳ thi đã hoàn thành, vui lòng kiểm tra lại", JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }
        
        public ActionResult ExportWord(int? idkt)
        {
            try
            {
                var model = (from dsp in db.DanhSachPhachs
                             join dst in db.DanhSachThis
                             on dsp.IdDanhSachThi equals dst.IdDanhSachThi
                             join kt in db.KyThis
                             on dst.IdKyThi equals kt.IdKyThi
                             where dst.IdKyThi == idkt
                             orderby dst.f_tenmhvn, dst.sobaodanh
                             select new
                             {
                                 f_masv = dst.f_masv,
                                 SoPhach = dsp.SoPhach,
                                 f_holotvn = dst.f_holotvn,
                                 f_tenvn = dst.f_tenvn,
                                 tenKyThi = kt.TenKyThi,
                                 monThi = dst.f_tenmhvn
                             }).ToList();
                rptPhachThi rpt = new rptPhachThi();
                DataTable dt = new DataTable();
                dt.Columns.Add("STT", typeof(int));
                dt.Columns.Add("MaSinhVien", typeof(String));
                dt.Columns.Add("HoTenSV", typeof(String));
                dt.Columns.Add("MaPhach", typeof(String));
                dt.Columns.Add("TenKyThi", typeof(String));
                dt.Columns.Add("MonThi", typeof(String));
                for (int i = 0; i < model.Count; i++)
                {
                    dt.Rows.Add(i + 1, model[i].f_masv, model[i].f_holotvn + " " + model[i].f_tenvn, model[i].SoPhach, model[i].tenKyThi, model[i].monThi);
                }
                rpt.Load();
                rpt.SetDataSource(dt);
                Stream s = rpt.ExportToStream(ExportFormatType.WordForWindows);
                return File(s, "application/docx", "Report.doc");
            }
            catch
            {
                return RedirectToAction("Index", "KyThi_Ad");
            }
        }

        private void CopyDataFormExcel(HttpPostedFileBase file, int? idKyThi)
        {
            var maKyThi = db.KyThis.Find(idKyThi).MaKyThi;
            var dtNow = DateTime.Now;
            string _FileName = maKyThi + "-" + dtNow.Year + dtNow.Month + dtNow.Day + dtNow.Hour + dtNow.Minute + dtNow.Second + ".xls";
            string _path = Path.Combine(Server.MapPath("~/Uploads/Excels"), _FileName);
            file.SaveAs(_path);
            DataTable dt = excelPro.ReadDataFromExcelFile(_path);
            dt.Columns.Add("KyThi", typeof(int));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][12] = idKyThi;
            }

            SqlBulkCopy bulkcopy = new SqlBulkCopy(con);
            bulkcopy.DestinationTableName = "DanhSachThis";
            bulkcopy.ColumnMappings.Add(0, "f_masv");
            bulkcopy.ColumnMappings.Add(1, "f_mamh");
            bulkcopy.ColumnMappings.Add(2, "f_holotvn");
            bulkcopy.ColumnMappings.Add(3, "f_tenvn");
            bulkcopy.ColumnMappings.Add(4, "f_ngaysinh");
            bulkcopy.ColumnMappings.Add(5, "sobaodanh");
            bulkcopy.ColumnMappings.Add(6, "f_tenlop");
            bulkcopy.ColumnMappings.Add(7, "f_tenmhvn");
            bulkcopy.ColumnMappings.Add(8, "ngaythi");
            bulkcopy.ColumnMappings.Add(9, "phongthi");
            bulkcopy.ColumnMappings.Add(10, "tietbatdau");
            bulkcopy.ColumnMappings.Add(11, "sotiet");
            bulkcopy.ColumnMappings.Add(12, "IdKyThi");
            con.Open();
            bulkcopy.WriteToServer(dt);
            con.Close();
        }

        private void InsertDataMonHoc(int? id)
        {
            var db = new KhaoThiDBContext();
            var model = db.DanhSachThis.Where(m => m.IdKyThi == id).Select(m => new { m.f_mamh, m.phongthi, m.ngaythi, m.tietbatdau, m.sotiet, m.IdKyThi, m.f_tenmhvn }).Distinct().ToList();
            for (int i = 0; i < model.Count; i++)
            {
                DanhSachThi dst = new DanhSachThi();
                dst.f_tenmhvn = model[i].f_tenmhvn;
                dst.f_masv = model[i].f_mamh;
                dst.f_holotvn = model[i].phongthi;
                dst.ngaythi = model[i].ngaythi;
                dst.tietbatdau = model[i].tietbatdau;
                dst.sotiet = model[i].sotiet;
                dst.IdKyThi = model[i].IdKyThi;
                dst.sobaodanh = 0;
                db.DanhSachThis.Add(dst);
            }
            db.SaveChanges();
        }

        private int InsertDataPhach1(int? id)
        {
            var listDanhSachThi = db.DanhSachThis.Where(m => m.IdKyThi == id).ToList();
            DataTable table = new DataTable();
            table.Columns.Add("SoPhach", typeof(string));
            table.Columns.Add("IdDanhSachThi", typeof(double));
            table.Columns.Add("IdKyThi", typeof(int));
            string[] arrCode = proLogic.GenBeatcodeWithQuantity(listDanhSachThi.Count, 5);
            for (int i = 0; i < arrCode.Length; i++)
            {
                table.Rows.Add(arrCode[i].ToString(), Convert.ToDouble(listDanhSachThi[i].IdDanhSachThi), id);
            }
            //copy du lieu
            CopyDataByBulk(table);
            //var countPhach = db.DanhSachPhachs.Where(m => m.IdKyThi == id).Select(m => m.SoPhach).Distinct().ToList().Count;
            var countPhach = table.Rows.Count;
            return countPhach;
        }

        //group by Monhoc
        private int InsertDataPhach(int? id)
        {
            try
            {
                //get list mon thi
                var listMonThi = db.DanhSachThis.Where(m => m.IdKyThi == id).Select(m => m.f_tenmhvn).Distinct().ToList();
                //table chua danh sach phach
                DataTable table = new DataTable();
                table.Columns.Add("SoPhach", typeof(string));
                table.Columns.Add("IdDanhSachThi", typeof(double));
                table.Columns.Add("IdKyThi", typeof(int));
                int startPhach = 1;
                for (int i = 0; i < listMonThi.Count; i++)
                {
                    var tkp = new ThongKePhach();
                    var tenMon = listMonThi[i].ToString();
                    //bug fixing
                    var listDanhSachThi = db.DanhSachThis.Where(m => m.IdKyThi == id && m.f_tenmhvn == tenMon).ToList();
                    //string[] arrCode = proLogic.GenBeatcodeWithQuantity(listDanhSachThi.Count, 5);
                    string[] arrCode = proLogic.GenerateBeatcode(startPhach, startPhach + listDanhSachThi.Count, 5);
                    for (int j = 0; j < arrCode.Length; j++)
                    {
                        table.Rows.Add(arrCode[j].ToString(), Convert.ToDouble(listDanhSachThi[j].IdDanhSachThi), id);
                    }
                    tkp.TenMonThi = tenMon;
                    tkp.PhachBatDau = startPhach;
                    //tinh gia tri bat dau sinh phach cua mon tiep theo
                    startPhach += listDanhSachThi.Count;
                    tkp.PhachKetThuc = startPhach - 1;
                    tkp.IdKyThi = id;
                    db.ThongKePhachs.Add(tkp);
                    db.SaveChanges();
                }
                //copy du lieu
                CopyDataByBulk(table);
                var countPhach = table.Rows.Count;
                return countPhach;
            }
            catch (Exception)
            {
                return 0;
            }
            
        }

        private int InsertDataPhachBoSung(int? id)
        {
            var lstThiSinhBoSung = (from dst in db.DanhSachThis
                                    where ((dst.IdKyThi == id) && (!db.DanhSachPhachs.Any(m => m.IdDanhSachThi == dst.IdDanhSachThi)))
                                    select new { idDST = dst.IdDanhSachThi }).ToList();
            DataTable table = new DataTable();
            table.Columns.Add("SoPhach", typeof(string));
            table.Columns.Add("IdDanhSachThi", typeof(double));
            table.Columns.Add("IdKyThi", typeof(int));
            var lstThiSinhCoSan = (from dst in db.DanhSachThis
                                   where ((dst.IdKyThi == id) && (db.DanhSachPhachs.Any(m => m.IdDanhSachThi == dst.IdDanhSachThi)))
                                   select new { idDST = dst.IdDanhSachThi }).ToList();
            string[] arrCode = proLogic.GenerateBeatcode(lstThiSinhCoSan.Count + 1, (lstThiSinhCoSan.Count + 1) + lstThiSinhBoSung.Count, 5);
            for (int i = 0; i < arrCode.Length; i++)
            {
                table.Rows.Add(arrCode[i].ToString(), Convert.ToDouble(lstThiSinhBoSung[i].idDST), id);
            }
            //copy du lieu
            CopyDataByBulk(table);
            var countPhach = table.Rows.Count;
            return countPhach;
        }

        private void AppendDataPhach(int? id)
        {
            var listDanhSachThi = db.DanhSachThis.Where(m => m.IdKyThi == id).ToList();
            DataTable table = new DataTable();
            table.Columns.Add("SoPhach", typeof(string));
            table.Columns.Add("IdDanhSachThi", typeof(double));
            table.Columns.Add("IdKyThi", typeof(int));
            string[] arrCode = proLogic.GenBeatcodeWithQuantity(listDanhSachThi.Count, 5);
            for (int i = 0; i < arrCode.Length; i++)
            {
                table.Rows.Add(arrCode[i].ToString(), Convert.ToDouble(listDanhSachThi[i].IdDanhSachThi), id);
            }
            var x = table.Rows.Count;
            SqlBulkCopy bulkcopy = new SqlBulkCopy(con);
            bulkcopy.DestinationTableName = "DanhSachPhachs";
            bulkcopy.ColumnMappings.Add(0, "SoPhach");
            bulkcopy.ColumnMappings.Add(1, "IdDanhSachThi");
            bulkcopy.ColumnMappings.Add(2, "IdKyThi");
            con.Open();
            bulkcopy.WriteToServer(table);
            con.Close();
            var countPhach = db.DanhSachPhachs.Where(m => m.IdKyThi == id).Select(m => m.SoPhach).Distinct().ToList().Count;
        }

        private void CopyDataByBulk(DataTable dt)
        {
            SqlBulkCopy bulkcopy = new SqlBulkCopy(con);
            bulkcopy.DestinationTableName = "DanhSachPhachs";
            bulkcopy.ColumnMappings.Add(0, "SoPhach");
            bulkcopy.ColumnMappings.Add(1, "IdDanhSachThi");
            bulkcopy.ColumnMappings.Add(2, "IdKyThi");
            con.Open();
            bulkcopy.WriteToServer(dt);
            con.Close();
        }
    }
}