using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KhaoThiSoftware.Models;

namespace KhaoThiSoftware.Areas.Lectures.Controllers
{
    public class KyThi_LeController : Controller
    {
        [Authorize(Roles = "Lecture")]
        public ActionResult Index()
        {
            using (var db = new KhaoThiDBContext())
            {
                var model = db.KyThis.Where(m => m.isDelete == false).ToList();
                return View(model);
            }
        }
    }
}