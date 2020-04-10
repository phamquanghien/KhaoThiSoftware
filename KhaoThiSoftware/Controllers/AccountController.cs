using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KhaoThiSoftware.Models;
using System.Web.Security;

namespace KhaoThiSoftware.Controllers
{
    public class AccountController : Controller
    {
        StringProcess strPro = new StringProcess();
        public ActionResult Create()
        {
            using (var db = new KhaoThiDBContext())
            {
                if (db.Accounts.Count() == 0)
                {
                    var role = new Role();
                    role.RoleID = "Admin";
                    role.RoleName = "Admin";
                    db.Roles.Add(role);
                    db.SaveChanges();
                    var acc = new Account();
                    acc.UserName = "admin";
                    acc.Password = strPro.GetMD5("123456");
                    acc.RoleID = "Admin";
                    db.Accounts.Add(acc);
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "Home");
            }
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
            if (ModelState.IsValid)
            {
                using (var db = new KhaoThiDBContext())
                {
                    var passToMD5 = strPro.GetMD5(acc.Password);
                    var account = db.Accounts.Where(m => m.UserName.Equals(acc.UserName) && m.Password.Equals(passToMD5)).ToList();
                    if (account.Count() == 1)
                    {
                        FormsAuthentication.SetAuthCookie(acc.UserName, false);
                        Session["idUser"] = account.FirstOrDefault().UserName;
                        return RedirectToLocal(returnUrl);
                    }
                }
            }
            ModelState.AddModelError("", "invalid Username or Password");
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