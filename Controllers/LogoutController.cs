using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class LogoutController : Controller
    {
        // GET: Logout
        public ActionResult Index()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}