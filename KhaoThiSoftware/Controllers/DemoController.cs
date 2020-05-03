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
            var model = db.DanhSachThis.Where(m => m.IdKyThi == 1).Select(m => new { m.f_mamh, m.phongthi, m.ngaythi, m.tietbatdau, m.sotiet, m.IdKyThi, m.f_tenmhvn }).Distinct().ToList();
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
            ViewBag.MyList = model.Count;
            return View(db.DanhSachThis.Where(m => m.IdKyThi == 1).OrderBy(m => m.f_tenmhvn).ThenBy(m => m.sobaodanh).ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
    }
}