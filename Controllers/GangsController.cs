using AdminPanel.Helpers;
using AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class GangsController : Controller
    {
        public ActionResult Index(string gangSearchName, string gangCount)
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Checks access level of the user, to see if they can acess this page
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.ViewGangs))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            Gangs gangs = new Gangs();
            gangs.gangSearchName = gangSearchName;

            if (gangCount == null)
            {
                gangs.gangCount = 15;
            }
            else
            {
                gangs.gangCount = Int32.Parse(gangCount);
            }

            return View(gangs);
        }

        public ActionResult EditGang(string id)
        {

            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Checks access level of the user, to see if they can acess this page
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.ViewGangs))
            {
                return RedirectToAction("Index", "Dashboard");
            }            

            if (id == null)
            {
                return View("Index");
            }
            else
            {
                EditGang editGang = new EditGang();
                editGang.id = Convert.ToInt32(id);
                editGang.setInfo();
                return View("EditGang", editGang);
            }
        }

        [HttpPost]
        public ActionResult UpdateStats(EditGang editGang)
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            editGang.updateStats();
            return RedirectToAction("EditGang", "Gangs", new { @id = editGang.id });
        }

    }
}