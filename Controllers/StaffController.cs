using AdminPanel.Helpers;
using AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class StaffController : Controller
    {
        public ActionResult Index(string staffSearchName, string staffCount)
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Checks access level of the user, to see if they can acess this page
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.ViewStaff))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            Staff staff = new Staff();
            staff.staffSearchName = staffSearchName;

            if (staffCount == null)
            {
                staff.staffCount = 15;
            }
            else
            {
                staff.staffCount = Int32.Parse(staffCount);
            }

            return View(staff);
        }

        public ActionResult EditStaff(string id)
        {

            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Checks access level of the user, to see if they can acess this page
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.EditStaffInfo))
            {
                return RedirectToAction("Index", "Dashboard");
            }            

            if (id == null)
            {
                return View("Index");
            }
            else
            {
                EditStaff editStaff = new EditStaff();
                editStaff.id = Convert.ToInt32(id);
                editStaff.setInfo();
                return View("EditStaff", editStaff);
            }
        }

        [HttpPost]
        public ActionResult UpdateInfo(EditStaff editStaff)
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Checks access level of the user, to see if they can acess this page
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.EditStaffInfo))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            editStaff.updateInfo();
            return RedirectToAction("EditStaff", "Staff", new { id = editStaff.id });
        }

        [HttpPost]
        public ActionResult CreateStaff(Staff staff)
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Checks access level of the user, to see if they can acess this page
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.CreateStaff))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            if (staff.newStaffPassword == staff.newStaffPasswordRepeat)
            {
                staff.createNewStaff();
            }

            return RedirectToAction("Index", "Staff");
        }
    }
}