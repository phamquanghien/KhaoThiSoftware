using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KhaoThiSoftware.Models;
using PagedList;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;

namespace KhaoThiSoftware.Controllers
{
    public class DemoController : Controller
    {
        KhaoThiDBContext db = new KhaoThiDBContext();
        ExcelProcess excelPro = new ExcelProcess();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["KhaoThiDBContext"].ConnectionString);
        // GET: Demo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(int? size, int? page)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "50", Value = "50" });
            items.Add(new SelectListItem { Text = "100", Value = "100" });
            items.Add(new SelectListItem { Text = "500", Value = "500" });
            items.Add(new SelectListItem { Text = "1000", Value = "1000" });
            items.Add(new SelectListItem { Text = "5000", Value = "5000" });
            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }
            // 1.2. Tạo các biến ViewBag
            ViewBag.size = items; // ViewBag DropDownList
            ViewBag.currentSize = size; // tạo biến kích thước trang hiện tại
                                        // 2. Nếu page = null thì đặt lại là 1.
            page = page ?? 1; //if (page == null) page = 1;

            // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo LinkID mới có thể phân trang.
            var links = (from l in db.DanhSachThis
                         select l).OrderBy(x => x.IdDanhSachThi);

            // 4. Tạo kích thước trang (pageSize), mặc định là 5.
            int pageSize = (size ?? 5);

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            // 5. Trả về các Link được phân trang theo kích thước và số trang.
            return View(links.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult UploadFile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, int? idKyThi)
        {
            idKyThi = 1;
            var maKyThi = db.KyThis.Find(idKyThi).MaKyThi;
            //try
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
                return View();
            }
            //catch
            //{
            //    ViewBag.Message = "File upload failed!!";
            //    return View();
            //}
        }

        public ActionResult GenPhach()
        {
            KhaoThiDBContext db = new KhaoThiDBContext();

            List<DanhSachPhach> allPhach = new List<DanhSachPhach>();
            allPhach = db.DanhSachPhachs.ToList();


            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/rpt"), "GenPhach.rpt"));

            rd.SetDataSource(allPhach);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Report.pdf");
        }

        public ActionResult GenPhach2()
        {
            KhaoThiDBContext db = new KhaoThiDBContext();

            var allPhach = (from dsp in db.DanhSachPhachs
                         join dst in db.DanhSachThis
                         on dsp.IdDanhSachThi equals dst.IdDanhSachThi
                         select new {
                             f_masv = dst.f_masv,
                             SoPhach = dsp.SoPhach,
                             f_holotvn = dst.f_holotvn,
                             f_tenvn = dst.f_tenvn 
                         }).ToList();
            
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/rpt"), "GenPhach.rpt"));

            rd.SetDataSource(allPhach);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Report.pdf");
        }

        public ActionResult GetDataReport()
        {
            List<Table> table = new List<Table>();
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Repport/GenerateBarcode.rpt")));

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);


            return File(stream, "application/pdf", "Report.pdf");
        }
    }
}