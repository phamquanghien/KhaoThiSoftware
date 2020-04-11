using System;
using System.Linq;
using System.Web.Mvc;
using KhaoThiSoftware.Models;
using System.Data.OleDb;
using System.Data;
using System.Web;
using System.IO;

namespace KhaoThiSoftware.Areas.Admins.Controllers
{
    public class DanhSachThi_AdController : Controller
    {
        KhaoThiDBContext db = new KhaoThiDBContext();
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
        public ActionResult UpLoadFile(HttpPostedFileBase file, int idKyThi)
        {
            var makyThi = db.KyThis.Find(idKyThi).MaKyThi;
            if (file.ContentLength > 0)
            {
                var fileName = makyThi + DateTime.Now.ToString();
                var path = Path.Combine(Server.MapPath("~/Uploads/Excels"), fileName);
                file.SaveAs(path);
            }
            return RedirectToAction("Index");
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

        public ActionResult Upload(int idKyThi)
        {
            string path = @"D:\Demo.xls";
            DataTable dt = ReadDataFromExcelFile(path);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var dst = new DanhSachThi();
                dst.f_masv = dt.Rows[i][0].ToString();
                dst.f_mamh = dt.Rows[i][1].ToString();
                dst.f_holotvn = dt.Rows[i][2].ToString();
                dst.f_tenvn = dt.Rows[i][3].ToString();
                dst.f_ngaysinh = dt.Rows[i][4].ToString();
                dst.sobaodanh = Convert.ToInt32(dt.Rows[i][5].ToString());
                dst.f_tenlop = dt.Rows[i][6].ToString();
                dst.f_tenmhvn = dt.Rows[i][7].ToString();
                dst.ngaythi = Convert.ToDateTime(dt.Rows[i][8].ToString());
                dst.phongthi = dt.Rows[i][9].ToString();
                dst.tietbatdau = Convert.ToInt32(dt.Rows[i][10].ToString());
                dst.sotiet = Convert.ToInt32(dt.Rows[i][11].ToString());
                dst.IdKyThi = idKyThi;
                db.DanhSachThis.Add(dst);
            }
            db.SaveChanges();
            ViewBag.countRow = dt.Rows.Count;
            return View();
        }

        private DataTable ReadDataFromExcelFile(string pathFile)
        {
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathFile + ";Extended Properties=Excel 8.0";
            // Tạo đối tượng kết nối
            OleDbConnection oledbConn = new OleDbConnection(connectionString);
            DataTable data = null;
            try
            {
                // Mở kết nối
                oledbConn.Open();

                // Tạo đối tượng OleDBCommand và query data từ sheet có tên "Sheet1"
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn);

                // Tạo đối tượng OleDbDataAdapter để thực thi việc query lấy dữ liệu từ tập tin excel
                OleDbDataAdapter oleda = new OleDbDataAdapter();

                oleda.SelectCommand = cmd;

                // Tạo đối tượng DataSet để hứng dữ liệu từ tập tin excel
                DataSet ds = new DataSet();

                // Đổ đữ liệu từ tập excel vào DataSet
                oleda.Fill(ds);

                data = ds.Tables[0];
            }
            catch (Exception ex)
            {
            }
            finally
            {
                // Đóng chuỗi kết nối
                oledbConn.Close();
            }
            return data;
        }
    }
}