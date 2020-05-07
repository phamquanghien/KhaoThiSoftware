using System.Linq;
using System.Web.Mvc;
using KhaoThiSoftware.Models;
using System.Web.Security;

namespace KhaoThiSoftware.Controllers
{
    public class AccountController : Controller
    {
        StringProcess strPro = new StringProcess();
        public ActionResult ResetPass()
        {
            using (var db = new KhaoThiDBContext())
            {
                var account = db.Accounts.Find("Admin");
                account.Password = strPro.GetMD5("123456");
                account.ConfirmPassword = account.Password;
                db.SaveChanges();
            }
            
            return RedirectToAction("Index", "Home_Ad");
        }
        public ActionResult Create()
        {
            try
            {
                using (var db = new KhaoThiDBContext())
                {
                    if (db.Roles.Count() == 0)
                    {
                        var role = new Role();
                        role.RoleID = "Admin";
                        role.RoleName = "Admin";
                        db.Roles.Add(role);
                        db.SaveChanges();
                    }
                    if (db.Accounts.Count() == 0)
                    {
                        var acc = new Account();
                        acc.UserName = "admin";
                        acc.Password = strPro.GetMD5("123456");
                        acc.ConfirmPassword = acc.Password;
                        acc.RoleID = "Admin";
                        db.Accounts.Add(acc);
                        db.SaveChanges();
                    }
                    
                }
            }
            catch {}
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(Account acc, string returnUrl)
        {
            try
            {
                if (!string.IsNullOrEmpty(acc.UserName) && !string.IsNullOrEmpty(acc.Password))
                {
                    using (var db = new KhaoThiDBContext())
                    {
                        var passToMD5 = strPro.GetMD5(acc.Password);
                        var account = db.Accounts.Where(m => m.UserName.Equals(acc.UserName) && m.Password.Equals(passToMD5)).Count();
                        if (account == 1)
                        {
                            FormsAuthentication.SetAuthCookie(acc.UserName, false);
                            Session["idUser"] = acc.UserName;
                            return RedirectToLocal(returnUrl);
                        }
                    }
                }
                ModelState.AddModelError("", "Thông tin đăng nhập chưa chính xác");
            }
            catch
            {
                ModelState.AddModelError("", "");
            }
            
            return View(acc);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}