using AdminPanel.Helpers;
using AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class SupportCasesController : Controller
    {
        // GET: SupportCases
        public ActionResult Index(string caseSearch, string caseCount)
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Checks access level of the user, to see if they can acess this page
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.CreateSupportCase))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            Models.SupportCases supportCases = new Models.SupportCases();

            supportCases.caseSearch = caseSearch;

            if (caseCount == null)
            {
                supportCases.caseCount = 15;
            } else
            {
                supportCases.caseCount = Convert.ToInt32(caseCount);
            }

            return View(supportCases);
        }

        [HttpPost]
        public ActionResult CreateSupportCase(Models.SupportCases supportCases)
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Checks access level of the user, to see if they can acess this page
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.CreateSupportCase))
            {
                return RedirectToAction("Index", "Dashboard");
            }


            if (supportCases != null)
            {
                supportCases.addNewCase();
                return RedirectToAction("Index", "SupportCases");
            } else
            {
                return View("Index");
            }
        }

        public ActionResult EditCase(string id)
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Checks access level of the user, to see if they can acess this page
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.CreateSupportCase))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            EditCase editCase = new EditCase();
            editCase.id = Convert.ToInt32(id);
            editCase.setInfo();

            //Checks if user is trying to access someone elses case and checks if they have permission to do so
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.EditOthersSupportCase) && editCase.staffUsername != HttpContext.Session["username"].ToString())
            {
                return RedirectToAction("Index", "SupportCases");
            }

            return View(editCase);
        }

        public ActionResult CloseCase(string id)
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Checks access level of the user, to see if they can acess this page
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.CreateSupportCase))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            EditCase editCase = new EditCase();
            editCase.id = Convert.ToInt32(id);
            editCase.setInfo();

            //Checks if user is trying to access someone elses case and checks if they have permission to do so
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.EditOthersSupportCase) && editCase.staffUsername != HttpContext.Session["username"].ToString())
            {
                return RedirectToAction("Index", "SupportCases");
            }

            editCase.closeCase();
            
            return RedirectToAction("EditCase", "SupportCases", new { id = editCase.id });
        }

        public ActionResult OpenCase(string id)
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Checks access level of the user, to see if they can acess this page
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.CreateSupportCase))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            EditCase editCase = new EditCase();
            editCase.id = Convert.ToInt32(id);
            editCase.setInfo();

            //Checks if user is trying to access someone elses case and checks if they have permission to do so
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.EditOthersSupportCase) && editCase.staffUsername != HttpContext.Session["username"].ToString())
            {
                return RedirectToAction("Index", "SupportCases");
            }

            editCase.openCase();
            
            return RedirectToAction("EditCase", "SupportCases", new { id = editCase.id });
        }

        public ActionResult DeleteCase(string id)
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Checks access level of the user, to see if they can acess this page
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.DeleteSupportCase))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            EditCase editCase = new EditCase();
            editCase.id = Convert.ToInt32(id);
            editCase.setInfo();

            //Checks if user is trying to access someone elses case and checks if they have permission to do so
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.EditOthersSupportCase) && editCase.staffUsername != HttpContext.Session["username"].ToString())
            {
                return RedirectToAction("Index", "SupportCases");
            }

            editCase.deleteCase();

            Models.SupportCases supportCases = new Models.SupportCases();
            supportCases.caseSearch = "";
            supportCases.caseCount = 5;

            return View("Index", supportCases);
        }
    }
}