using AdminPanel.Helpers;
using AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class VehiclesController : Controller
    {
        public ActionResult Index(string vehicleSearchPID, string vehicleCount)
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Checks access level of the user, to see if they can acess this page
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.ViewVehicles))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            Vehicles vehicle = new Vehicles();
            vehicle.vehicleSearchPID = vehicleSearchPID;

            if (vehicleCount == null)
            {
                vehicle.vehicleCount = 15;
            }
            else
            {
                vehicle.vehicleCount = Int32.Parse(vehicleCount);
            }
            
            return View(vehicle);
        }

        public ActionResult EditVehicle(string id)
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
                EditVehicle editVehicle = new EditVehicle();
                editVehicle.id = Convert.ToInt32(id);
                editVehicle.setStats();
                return View("EditVehicle", editVehicle);
            }
        }

        [HttpPost]
        public ActionResult UpdateStats(EditVehicle editVehicle)
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            editVehicle.updateStats();
            return RedirectToAction("EditVehicle", "Vehicles", new { @id = editVehicle.id });
        }
    }
}