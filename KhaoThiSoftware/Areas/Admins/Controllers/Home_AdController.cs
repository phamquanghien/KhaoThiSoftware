using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KhaoThiSoftware.Areas.Admins.Controllers
{
    [Authorize(Roles = "Admin")]
    public class Home_AdController : Controller
    {
        // GET: Admins/Home_Ad
        public ActionResult Index()
        {
            return RedirectToAction("Index", "KyThi_Ad", new { area = "Admins"});
        }
    }
}