using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AdminPanel.Helpers
{
    public static class Permissions
    {
        public enum perms
        {
            [Display(Name = "View Players")]
            ViewPlayers,
            [Display(Name = "View Vehicles")]
            ViewVehicles,
            [Display(Name = "View Houses")]
            ViewHouses,
            [Display(Name = "View Gangs")]
            ViewGangs,
            [Display(Name = "View Staff")]
            ViewStaff,
            [Display(Name = "Access Dashboard")]
            AccessDashboard,
            [Display(Name = "Edit Player NATO Level")]
            EditPlayerNATOLevel,
            [Display(Name = "Edit Player Medic Level")]
            EditPlayerMedicLevel,
            [Display(Name = "Edit Player Admin Level")]
            EditPlayerAdminLevel,
            [Display(Name = "Edit Player Cash")]
            EditPlayerCash,
            [Display(Name = "Edit Player Bank")]
            EditPlayerBank,
            [Display(Name = "Edit Player Arrested")]
            EditPlayerArrested,
            [Display(Name = "Edit Player Blacklist")]
            EditPlayerBlacklist,
            [Display(Name = "Edit Player Alive")]
            EditPlayerAlive,
            [Display(Name = "Edit Player Licenses")]
            EditPlayerLicenses,
            [Display(Name = "Edit Player Gear")]
            EditPlayerGear,
            [Display(Name = "Edit Vehicle Alive")]
            EditVehicleAlive,
            [Display(Name = "Edit Vehicle Blacklist")]
            EditVehicleBlacklist,
            [Display(Name = "Edit Vehicle Active")]
            EditVehicleActive,
            [Display(Name = "Edit Vehicle Colour")]
            EditVehicleColour,
            [Display(Name = "Edit Vehicle Inventory")]
            EditVehicleInventory,
            [Display(Name = "Edit Vehicle Gear")]
            EditVehicleGear,
            [Display(Name = "Edit Gang Owner")]
            EditGangOwner,
            [Display(Name = "Edit Gang Name")]
            EditGangName,
            [Display(Name = "Edit Gang Max Members")]
            EditGangMaxMembers,
            [Display(Name = "Edit Gang Bank")]
            EditGangBank,
            [Display(Name = "Edit Gang Active")]
            EditGangActive,
            [Display(Name = "Create Staff")]
            CreateStaff,
            [Display(Name = "Create Staff Note")]
            CreateStaffNote,
            [Display(Name = "Create Support Case")]
            CreateSupportCase,
            [Display(Name = "Edit Staff Info")]
            EditStaffInfo,
            [Display(Name = "Edit Others Support Case")]
            EditOthersSupportCase,
            [Display(Name = "Delete Support Case")]
            DeleteSupportCase,
            [Display(Name = "Add Warning Points")]
            AddWarningPoints,
            [Display(Name = "Create staff rank")]
            CreateStaffRank,
            [Display(Name = "View settings")]
            ViewSettings
        };

        public enum accessLevels
        {
            TrialStaff,
            Moderator,
            Administrator,
            SeniorAdministrator,
            HeadAdministrator,
            StaffManager,
            Director,
            Owner
        }

        public static bool hasPermission(string accesslevel, Permissions.perms perm)
        {
            List<string> ranksList = new List<string>();

            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string sql = "SELECT perms FROM ranks_panel WHERE name=@name";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@name", accesslevel);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string perms = reader.GetString(0);
                    ranksList = perms.Split(',').ToList();
                }

                reader.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();

            if (ranksList.Contains(perm.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<string> getRanks()
        {
            List<string> ranks = new List<string>();

            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string sql = "SELECT name FROM ranks_panel";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //Gets all the player stats from the database
                    ranks.Add(reader.GetString(0));
                }

                reader.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();

            return ranks;
        }
    }
}