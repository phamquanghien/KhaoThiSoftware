using CrystalDecisions.CrystalReports.Engine;
using KhaoThiSoftware.Datasets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KhaoThiSoftware.Models;

namespace KhaoThiSoftware.Reports
{
    public partial class DisplayReport : System.Web.UI.Page
    {
        KhaoThiDBContext db = new KhaoThiDBContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportDocument report = new ReportDocument();
            KhaoThiDataset ds = new KhaoThiDataset();
            ds.Tables["DanhSachPhach"].Merge(GetAllDSP());
            report.Load(Server.MapPath("~/Reports/rpt/rptPhachThi.rpt"));
            report.SetDataSource(ds);
            CrystalReportViewer1.ReportSource = report;
            CrystalReportViewer1.DisplayToolbar = true;
        }
        public DataTable GetAllDSP()
        {
            var model = (from dsp in db.DanhSachPhachs
                         join dst in db.DanhSachThis
                         on dsp.IdDanhSachThi equals dst.IdDanhSachThi
                         join kt in db.KyThis
                         on dst.IdKyThi equals kt.IdKyThi
                         //where dst.IdKyThi == idkt
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
            return dt;
        }
    }
}