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
            try
            {
                using (var db = new KhaoThiDBContext())
                {
                    var account = db.Accounts.Find("Admin");
                    account.Password = strPro.GetMD5("123456");
                    account.ConfirmPassword = account.Password;
                    db.SaveChanges();
                }
            }
            catch { }
            
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

                        var role2 = new Role();
                        role2.RoleID = "Lecture";
                        role2.RoleName = "Giảng viên";
                        db.Roles.Add(role2);
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
            return RedirectToAction("Login", "Account");
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (CheckSession() == 1)
            {
                return RedirectToAction("Index", "Home_Ad", new { Area = "Admins" });
            }
            else if (CheckSession() == 2)
            {
                return RedirectToAction("Index", "Home_Le", new { Area = "Lectures" });
            }
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
                            Session["roleUser"] = acc.RoleID;
                            
                            return RedirectToLocal(returnUrl);
                        }
                        ModelState.AddModelError("", "Thông tin đăng nhập chưa chính xác");
                    }
                }
                ModelState.AddModelError("", "Username and password is required.");
            }
            catch
            {
                ModelState.AddModelError("", "Hệ thống đang được bảo trì, vui lòng liên hệ với quản trị viên");
            }
            
            return View(acc);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["idUser"] = null;
            return RedirectToAction("Login", "Account");
        }
        
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || returnUrl =="/")
            {
                if (CheckSession() == 1)
                {
                    return RedirectToAction("Index", "Home_Ad", new { Areas = "Admins" });
                }
                else if (CheckSession() == 2)
                {
                    return RedirectToAction("Index", "Home_Le", new { Areas = "Lectures" });
                }
            }
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home_Ad");
            }
        }

        private int CheckSession()
        {
            using (var db = new KhaoThiDBContext())
            {
                var user = HttpContext.Session["idUser"];
                if (user != null)
                {
                    var role = db.Accounts.Find(user.ToString()).RoleID;
                    if (role != null)
                    {
                        if (role.ToString() == "Admin")
                        {
                            return 1;
                        }
                        else if (role.ToString() == "Lecture")
                        {
                            return 2;
                        }
                    }
                }
                
            }
            return 0;
        }
    }
}