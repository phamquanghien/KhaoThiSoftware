using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using KhaoThiSoftware.Models;
using System.Collections.Generic;

namespace KhaoThiSoftware.Areas.Admins.Controllers
{
    [Authorize(Roles = "Admin")]
    public class Account_AdController : Controller
    {
        private KhaoThiDBContext db = new KhaoThiDBContext();
        StringProcess strPro = new StringProcess();

        // GET: Admins/Account_Ad
        public ActionResult Index()
        {
            return View(db.Accounts.ToList());
        }

        // GET: Admins/Account_Ad/Create
        public ActionResult Create()
        {
            ViewBag.ListRole = db.Roles.ToList();
            return View();
        }

        // POST: Admins/Account_Ad/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserName,Password,RoleID,ConfirmPassword")] Account account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var md5Pass = strPro.GetMD5(account.Password);
                    account.Password = md5Pass;
                    account.ConfirmPassword = md5Pass;
                    db.Accounts.Add(account);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Username is exist. Please try again.");
                ViewBag.ListRole = db.Roles.ToList();
            }
            
            return View(account);
        }

        // GET: Admins/Account_Ad/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return View("~/Views/Shared/404Notfound.cshtml");
            }
            else
            {
                if (account.UserName == "admin")
                {
                    ViewBag.ThongBao = "Không thể xóa tài khoản có quyền Admin";
                }
                else
                {
                    ViewBag.ThongBao = "Bạn có chắc chắn muốn xóa tài khoản này không?";
                }
            }
            return View(account);
        }

        // POST: Admins/Account_Ad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Account account = db.Accounts.Find(id);
            if (account.UserName != "admin")
            {
                db.Accounts.Remove(account);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Admins/Role_Ad/Edit/5
        public ActionResult ChangePassWord(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return View("~/Views/Shared/404Notfound.cshtml");
            }
            return View(account);
        }

        // POST: Admins/Role_Ad/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassWord(Account account)
        {
            if (ModelState.IsValid)
            {
                string md5Pass = strPro.GetMD5(account.Password);
                account.Password = md5Pass;
                account.ConfirmPassword = md5Pass;
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
