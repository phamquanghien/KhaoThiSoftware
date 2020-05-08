using System.Web.Mvc;
using KhaoThiSoftware.Models;

namespace KhaoThiSoftware.Controllers
{
    public class HomeController : Controller
    {
        KhaoThiDBContext db = new KhaoThiDBContext();
        //[Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}