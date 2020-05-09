using System.Web.Mvc;
using KhaoThiSoftware.Models;
using System.Linq;

namespace KhaoThiSoftware.Controllers
{
    public class HomeController : Controller
    {
        KhaoThiDBContext db = new KhaoThiDBContext();
        //[Authorize]
        public ActionResult Index()
        {
            var model = db.DanhSachThis.Where(m => m.IdKyThi == 1).Select(m => m.f_tenmhvn).Distinct().ToList();
            ViewBag.DanhSach = model;
            return View();
        }
    }
}