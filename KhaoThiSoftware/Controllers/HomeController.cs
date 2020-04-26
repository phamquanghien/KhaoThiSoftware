using System.Web.Mvc;
using KhaoThiSoftware.Models;
using System.Collections.Generic;
using System.Linq;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using KhaoThiSoftware.Reports.rpt;

namespace KhaoThiSoftware.Controllers
{
    public class HomeController : Controller
    {
        KhaoThiDBContext db = new KhaoThiDBContext();
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home_Ad", new { area = "Admins" });
            //return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult DemoReportViewer()
        {
            return View();
        }
        public ActionResult ExportPDF()
        {
            List<DanhSachPhach> allPhach = new List<DanhSachPhach>();
            allPhach = db.DanhSachPhachs.ToList();


            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/rpt"), "rptDanhSachPhach.rpt"));

            rd.SetDataSource(allPhach);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Report.pdf");
        }

        public ActionResult ExportWord()
        {
            var model = db.DanhSachPhachs.Where(m => m.IdDanhSachPhach < 50).ToList();
            rptDanhSachPhach rpt = new rptDanhSachPhach();
            rpt.Load();
            rpt.SetDataSource(model);
            //Stream s = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //return File(s, "application/pdf");
            Stream s = rpt.ExportToStream(ExportFormatType.WordForWindows);
            return File(s, "application/docx", "Report.doc");
        }
        public ActionResult ExportWord2()
        {
            var model = (from dsp in db.DanhSachPhachs
                         join dst in db.DanhSachThis
                         on dsp.IdDanhSachThi equals dst.IdDanhSachThi
                         where dsp.IdDanhSachPhach < 50
                         select new
                         {
                             f_masv = dst.f_masv,
                             SoPhach = dsp.SoPhach,
                             f_holotvn = dst.f_holotvn,
                             f_tenvn = dst.f_tenvn
                         }).ToList();
            var danhSachThi = db.DanhSachThis.ToList();
            var danhSachPhach = db.DanhSachPhachs.ToList();
            rptPhachThi rpt = new rptPhachThi();
            rpt.Load();
            rpt.SetDataSource(model);
            //rpt.Database.Tables["KhaoThiSoftware_Model_DanhSachThi"].SetDataSource(danhSachThi);
            //rpt.Database.Tables["DanhSachPhach"].SetDataSource(danhSachPhach);
            Stream s = rpt.ExportToStream(ExportFormatType.WordForWindows);
            return File(s, "application/docx", "Report.docx");
        }
    }
}