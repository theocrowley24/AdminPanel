using AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                Dashboard dashboard = new Dashboard();
                dashboard.setServerStats();
                return View(dashboard);
            }            
        }
    }
}