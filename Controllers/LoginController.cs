using AdminPanel.Helpers;
using AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index(User user = null)
        {

            if (user == null)
            {
                return View("Index");
            } else
            {
                return View("Index", user);
            }

            
            
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            Login login = new Login(user.Username, user.Password);
            if (login.verified)
            {
                return RedirectToAction("Index", "Dashboard");
            } else
            {
                user.verified = false;
                user.errorMessage = login.errorMessage;
                return View("Index", user);
            }            
        }
    }
}