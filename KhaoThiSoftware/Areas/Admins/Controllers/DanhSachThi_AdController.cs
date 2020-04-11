using System;
using System.Linq;
using System.Web.Mvc;
using KhaoThiSoftware.Models;
using System.Data.OleDb;
using System.Data;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;

namespace KhaoThiSoftware.Areas.Admins.Controllers
{
    public class DanhSachThi_AdController : Controller
    {
        KhaoThiDBContext db = new KhaoThiDBContext();
        ExcelProcess excelPro = new ExcelProcess();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["KhaoThiDBContext"].ConnectionString);
        // GET: Admins/DanhSachThi_Ad
        public ActionResult Index(int? id)
        {
            if (id == null) return RedirectToAction("Index", "KyThi_Ad");
            var model = db.DanhSachThis.Where(m => m.IdKyThi == id).ToList();
            ViewBag.sobanghi = model.Count;
            ViewBag.idKyThi = id;
            return View();
        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, int? idKyThi)
        {
            var maKyThi = db.KyThis.Find(idKyThi).MaKyThi;

            try
            {
                if (file.ContentLength > 0)
                {
                    //string _FileName = maKyThi + DateTime.Now.ToString();
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Uploads/Excels"), _FileName);
                    file.SaveAs(_path);
                    DataTable dt = excelPro.ReadDataFromExcelFile(_path);
                    DataColumn col = dt.Columns.Add("IdKyThi", typeof(Int32));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //column IDKYTHi
                        dt.Rows[i][12] = idKyThi;
                    }
                    //col.DefaultValue = idKyThi;
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
                ViewBag.Message = "File Uploaded Successfully!!";
                return RedirectToAction("Index", "DanhSachThi_Ad", new { id = idKyThi });
            }
            catch
            {
                return View();
            }
        }
        public JsonResult GetData(int id)
        {
            var model = db.DanhSachThis.Where(m => m.IdKyThi == id).Take(1000).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenPhach(int id)
        {
            var sophach = db.DanhSachThis.Where(m => m.IdKyThi == id);
            if (sophach.Count() == 0)
            {

            }
            return View();
        }
    }
}