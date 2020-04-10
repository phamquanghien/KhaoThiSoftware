using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KhaoThiSoftware.Models;

namespace KhaoThiSoftware.Areas.Admins.Controllers
{
    public class DanhSachThi_AdController : Controller
    {
        KhaoThiDBContext db = new KhaoThiDBContext();
        // GET: Admins/DanhSachThi_Ad
        public ActionResult GetListStudent(int id)
        {
            return View();
        }
    }
}