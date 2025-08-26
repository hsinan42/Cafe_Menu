using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Cafe_Menu.Controllers
{
    public class AdminController : Controller
    {
        AdminManager am = new AdminManager(new EfAdminDal());
        // GET: Admin
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(Admin p)
        {
            var admininfo = am.GetAdminByInfo(p);
            if (admininfo != null)
            {
                FormsAuthentication.SetAuthCookie(admininfo.AdminName, false);
                Session["AdminName"] = admininfo.AdminName;
                return RedirectToAction("AdminHome");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [Authorize]
        public ActionResult AdminHome()
        {
            return View();
        }
    }
}