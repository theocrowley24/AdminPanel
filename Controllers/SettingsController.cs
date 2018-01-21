using AdminPanel.Helpers;
using AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class SettingsController : Controller
    {
        // GET: Settings
        public ActionResult Index(Settings settings = null)
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Checks access level of the user, to see if they can acess this page
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.ViewSettings))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            if (settings == null)
            {
                return View(new Settings());
            } else
            {
                return View(settings);
            }            
        }

        [HttpPost]
        public ActionResult CreateRank(Settings settings)
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Checks access level of the user, to see if they can acess this page
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.CreateStaffRank))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            settings.createRank();
            return RedirectToAction("Index", "Settings", new Settings());
        }

        [HttpPost]
        public ActionResult SelectRank(Settings settings)
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Checks access level of the user, to see if they can acess this page
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.CreateStaffRank))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return RedirectToAction("Index", "Settings", settings);
        }

        [HttpPost]
        public ActionResult EditRank(Settings settings)
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Checks access level of the user, to see if they can acess this page
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.CreateStaffRank))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            settings.updateRank();
            return RedirectToAction("Index", "Settings", new Settings());
        }
    }
}