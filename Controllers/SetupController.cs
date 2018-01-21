using AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class SetupController : Controller
    {
        // GET: Setup
        public ActionResult Index(Setup setup = null)
        {
            try
            {
                if (HttpContext.Session["accessLevel"].ToString() == "root")
                {
                    if (setup == null)
                    {
                        return View(new Setup());
                    }
                    else
                    {
                        return View(setup);
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            if (ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString == ""){
                
                if (setup == null)
                {
                    return View();
                }
                else
                {
                    return View(setup);
                }
            }         
           

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public ActionResult Setup(Setup setup)
        {
            if (ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString != "")
            {
                return RedirectToAction("Index", "Dashboard");
            }

            setup.runSetup();
            if (setup.failed)
            {
                return View("Index", setup);
            } else
            {
                return RedirectToAction("Index", "Login");
            }            
        }
    }
}