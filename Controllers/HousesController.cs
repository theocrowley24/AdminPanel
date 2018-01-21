using AdminPanel.Helpers;
using AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class HousesController : Controller
    {
        public ActionResult Index(string houseSearchPID, string houseCount)
        {

            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Checks access level of the user, to see if they can acess this page
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.ViewHouses))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            Houses houses = new Houses();
            houses.houseSearchPID = houseSearchPID;

            if (houseCount == null)
            {
                houses.houseCount = 15;
            }
            else
            {
                houses.houseCount = Int32.Parse(houseCount);
            }

            return View(houses);
        }

    }
}