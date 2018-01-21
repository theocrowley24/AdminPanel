using AdminPanel.Helpers;
using AdminPanel.Models;
using System;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class PlayersController : Controller
    {
        
        public ActionResult Index(string playerSearchName, string playerCount)
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Checks access level of the user, to see if they can acess this page
            if (!Permissions.hasPermission(HttpContext.Session["accessLevel"].ToString(), Permissions.perms.ViewPlayers)){
                return RedirectToAction("Index", "Dashboard");
            }

            Players player = new Players();
            player.playerSearchName = playerSearchName;

            if (playerCount == null)
            {
                player.playerCount = 15;
            } else
            {
                player.playerCount = Convert.ToInt32(playerCount);
            }

            return View(player);
        }

        public ActionResult EditPlayer(string uid, string vehicleCount, string houseCount)
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

            if (uid == null)
            {
                return View("Index");
            } else
            {
                EditPlayer editPlayer = new EditPlayer();
                editPlayer.uid = Convert.ToInt32(uid);
                editPlayer.vehicleCount = Convert.ToInt32(vehicleCount);
                editPlayer.houseCount = Convert.ToInt32(houseCount);
                editPlayer.setInfo();
                return View("EditPlayer", editPlayer);
            }            
        }

        [HttpPost]
        public ActionResult UpdateStats(EditPlayer editPlayer)
        {
            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            editPlayer.updateStats();
            editPlayer.AddWarningPoints();
            return RedirectToAction("EditPlayer", "Players", new { @uid = editPlayer.uid });
        }

        [HttpPost]
        public ActionResult UpdateLicenses(string[] licenses, string uid)
        {

            //Checks that the user is logged in, if they aren't then they are redirected to the login page
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            EditPlayer editPlayer = new EditPlayer();
            editPlayer.uid = Convert.ToInt32(uid);
            editPlayer.setInfo();
            editPlayer.UpdateLicenses(licenses);
            return RedirectToAction("EditPlayer", "Players", new { @uid = editPlayer.uid });
        }

    }
}