using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using KhaoThiSoftware.Models;

namespace KhaoThiSoftware.Areas.Admins.Controllers
{
    public class Role_AdController : Controller
    {
        private KhaoThiDBContext db = new KhaoThiDBContext();

        // GET: Admins/Role_Ad
        public ActionResult Index()
        {
            return View(db.Roles.ToList());
        }
        
        // GET: Admins/Role_Ad/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admins/Role_Ad/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoleID,RoleName")] Role role)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: Admins/Role_Ad/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return View("~/Views/Shared/404Notfound.cshtml");
            }
            return View(role);
        }

        // POST: Admins/Role_Ad/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoleID,RoleName")] Role role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        // GET: Admins/Role_Ad/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return View("~/Views/Shared/404Notfound.cshtml");
            }
            if (role.RoleID == "Admin")
            {
                ViewBag.ThongBao = "Không thể xóa quyền Admin";
            }
            else if (db.Accounts.Where(m => m.RoleID == id).Count()>0)
            {
                ViewBag.ThongBao = "Tồn tại tài khoản liên kết với quyền hạn này, vui lòng kiểm tra lại";
            }
            else
            {
                ViewBag.ThongBao = "Bạn có muốn xóa quyền này không?";
            }
            
            return View(role);
        }

        // POST: Admins/Role_Ad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                Role role = db.Roles.Find(id);
                if (role.RoleID != "Admin" || db.Accounts.Where(m => m.RoleID == id).Count() > 0)
                {
                    db.Roles.Remove(role);
                    db.SaveChanges();
                }
            }
            catch { }
            
            return RedirectToAction("Index");
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
