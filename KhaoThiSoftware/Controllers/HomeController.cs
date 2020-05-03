﻿using System.Web.Mvc;
using KhaoThiSoftware.Models;
using System.Collections.Generic;
using System.Linq;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using KhaoThiSoftware.Reports.rpt;
using System.Data;
using System;

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
            var model = (from dsp in db.DanhSachPhachs
                         join dst in db.DanhSachThis
                         on dsp.IdDanhSachThi equals dst.IdDanhSachThi
                         join kt in db.KyThis
                         on dst.IdKyThi equals kt.IdKyThi
                         //where dsp.IdDanhSachPhach < 50
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
            //Stream s = rpt.ExportToStream(ExportFormatType.WordForWindows);
            //return File(s, "application/docx", "Report.doc");
            Stream s = rpt.ExportToStream(ExportFormatType.PortableDocFormat);
            return File(s, "application/pdf", "Report.pdf");
        }
        public ActionResult ExportWord()
        {
            var model = (from dsp in db.DanhSachPhachs
                         join dst in db.DanhSachThis
                         on dsp.IdDanhSachThi equals dst.IdDanhSachThi
                         join kt in db.KyThis
                         on dst.IdKyThi equals kt.IdKyThi
                         //where dsp.IdDanhSachPhach < 50
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
    }
}