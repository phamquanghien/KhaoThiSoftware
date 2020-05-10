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
            var model = db.DanhSachThis.OrderByDescending(m => m.f_tenmhvn).Select(m => m.f_tenmhvn).Distinct().ToList();
            model.Sort();
            return View(model);
        }
        public ActionResult Create()
        {
            return View();
        }
    }
}