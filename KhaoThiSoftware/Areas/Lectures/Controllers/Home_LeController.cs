using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KhaoThiSoftware.Areas.Lectures.Controllers
{
    public class Home_LeController : Controller
    {
        [Authorize(Roles = "Lecture")]
        // GET: Lectures/Home_Le
        public ActionResult Index()
        {
            return RedirectToAction("Index", "KyThi_Le");
        }
    }
}