using AdminPanel.Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AdminPanel.Models
{
    public class EditPlayer
    {
        public int uid { get; set; }
        public string name { get; set; }
        public string aliases { get; set; }
        public long pid { get; set; }
        public int cash { get; set; }
        public int bankAccount { get; set; }
        public int copLevel { get; set; }
        public int medicLevel { get; set; }
        public int adminLevel { get; set; }
        public string civLicenses { get; set; }
        public string copLicenses { get; set; }
        public string medicLicenses { get; set; }
        public string civGear { get; set; }
        public string copGear { get; set; }
        public string medicGear { get; set; }
        public int arrested { get; set; }
        public int blacklist { get; set; }
        public int alive { get; set; }
        public string playtime { get; set; }
        public string insertTime { get; set; }
        public string lastSeen { get; set; }
        public int vehicleCount { get; set; }
        public int houseCount { get; set; }
        public int warningPoints { get; set; }
        public int warningPointsToAdd { get; set; }

        public Dictionary<string, string> licenseDisplayNames = new Dictionary<string, string>()
            {
                { "license_civ_driver",  "Driver License"},
                { "license_civ_pilot", "Pilot License" },
                { "license_civ_boat", "Boating License" },
                { "license_civ_trucking", "Trucking License" },
                { "license_civ_gun", "Firearms License" },
                { "license_civ_dive", "Diving License" },
                { "license_civ_home", "Homeowners License" },
                { "license_civ_oil", "Oil Processing" },
                { "license_civ_diamond", "Diamond Processing" },
                { "license_civ_salt", "Salt Processing" },
                { "license_civ_sand", "Sand Processing" },
                { "license_civ_iron", "Iron Processing" },
                { "license_civ_copper", "Copper Processing" },
                { "license_civ_cement", "Cement Processing" },
                { "license_civ_medmarijuana", "Medical Marijuana License" },
                { "license_civ_cocaine", "Cocaine Processing" },
                { "license_civ_heroin", "Heroin Processing" },
                { "license_civ_marijuana", "Marijuana Processing" },
                { "license_civ_rebel", "Rebel Training" },
                { "license_civ_passport", "Passport" },
                { "license_cop_cAir", "NATO Pilot Training" },
                { "license_cop_cg", "Coast Guard" },
                { "license_med_mAir", "Medic Air" }

            };

        public void setInfo()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string sql = "SELECT * FROM players WHERE uid=@uid";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@uid", uid);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    //Gets all the player stats from the database
                    name = reader.GetString(1);
                    aliases = reader.GetString(2);
                    pid = reader.GetInt64(3);
                    cash = reader.GetInt32(4);
                    bankAccount = reader.GetInt32(5);
                    copLevel = reader.GetInt32(6);
                    medicLevel = reader.GetInt32(7);
                    civLicenses = reader.GetString(8);
                    copLicenses = reader.GetString(9);
                    medicLicenses = reader.GetString(10);
                    civGear = reader.GetString(11);
                    copGear = reader.GetString(12);
                    medicGear = reader.GetString(13);
                    arrested = reader.GetInt32(17);
                    adminLevel = reader.GetInt32(18);
                    blacklist = reader.GetInt32(20);
                    alive = reader.GetInt32(21);
                    playtime = reader.GetString(23);
                    insertTime = reader.GetString(24);
                    lastSeen = reader.GetString(25);
                    warningPoints = reader.GetInt32(reader.GetOrdinal("warning_points"));
                }

                reader.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();
        }

        public void updateStats()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            Database database = new Database();

            if (!Permissions.hasPermission(HttpContext.Current.Session["accessLevel"].ToString(), Permissions.perms.EditPlayerNATOLevel))
            {
                copLevel = Convert.ToInt32(database.getDatabaseValue("players", "coplevel", uid));
            }
            if (!Permissions.hasPermission(HttpContext.Current.Session["accessLevel"].ToString(), Permissions.perms.EditPlayerMedicLevel))
            {
                medicLevel = Convert.ToInt32(database.getDatabaseValue("players", "mediclevel", uid));
            }
            if (!Permissions.hasPermission(HttpContext.Current.Session["accessLevel"].ToString(), Permissions.perms.EditPlayerAdminLevel))
            {
                adminLevel = Convert.ToInt32(database.getDatabaseValue("players", "adminlevel", uid));
            }
            if (!Permissions.hasPermission(HttpContext.Current.Session["accessLevel"].ToString(), Permissions.perms.EditPlayerCash))
            {
                cash = Convert.ToInt32(database.getDatabaseValue("players", "cash", uid));
            }
            if (!Permissions.hasPermission(HttpContext.Current.Session["accessLevel"].ToString(), Permissions.perms.EditPlayerBank))
            {
                bankAccount = Convert.ToInt32(database.getDatabaseValue("players", "bankacc", uid));
            }
            if (!Permissions.hasPermission(HttpContext.Current.Session["accessLevel"].ToString(), Permissions.perms.EditPlayerArrested))
            {
                arrested = Convert.ToInt32(database.getDatabaseValue("players", "arrested", uid));
            }
            if (!Permissions.hasPermission(HttpContext.Current.Session["accessLevel"].ToString(), Permissions.perms.EditPlayerBlacklist))
            {
                blacklist = Convert.ToInt32(database.getDatabaseValue("players", "blacklist", uid));
            }
            if (!Permissions.hasPermission(HttpContext.Current.Session["accessLevel"].ToString(), Permissions.perms.EditPlayerAlive))
            {
                alive = Convert.ToInt32(database.getDatabaseValue("players", "civ_alive", uid));
            }
            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "UPDATE players SET cash=@cash, bankacc=@bankAccount, coplevel=@copLevel, " +
                    "mediclevel=@medicLevel, adminlevel=@adminLevel, arrested=@arrested, blacklist=@blacklist, " +
                    "civ_alive=@alive WHERE uid=@uid";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@cash", cash);
                cmd.Parameters.AddWithValue("@bankAccount", bankAccount);
                cmd.Parameters.AddWithValue("@copLevel", copLevel + 1);
                cmd.Parameters.AddWithValue("@medicLevel", medicLevel + 1);
                cmd.Parameters.AddWithValue("@adminLevel", adminLevel + 1);
                cmd.Parameters.AddWithValue("@arrested", arrested);
                cmd.Parameters.AddWithValue("@blacklist", blacklist);
                cmd.Parameters.AddWithValue("@alive", alive);
                cmd.Parameters.AddWithValue("@uid", uid);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();
        }

        //Converts a license string to a list of licenses
        public List<license> convertLicenses(string licenses)
        {
            List<license> licenseList = new List<license>();

            if (licenses == "\"[]\"")
            {
                return licenseList;
            } else
            {
                for (int i = 0; i < licenses.Length; i++)
                {
                    if (licenses[i].ToString() == "[" && licenses[i + 1].ToString() != "[")
                    {
                        int j = i + 1;
                        string licenseName = "";

                        while (licenses[j].ToString() != ",")
                        {
                            licenseName += licenses[j];
                            j++;
                        }

                        licenseName = licenseName.Replace("`", "");
                        licenseList.Add(new license(licenseName, Convert.ToInt32(licenses[j + 1]) - 48));
                    }
                }

                return licenseList;
            }

            
        }

        public void UpdateLicenses(string[] licenses)
        {
            //Creates a list containing all of the licenses (cop, civ, med)
            List<license> licenseList = new List<license>();
            licenseList = convertLicenses(civLicenses).Concat(convertLicenses(copLicenses)).Concat(convertLicenses(medicLicenses)).ToList();

            //Coverts the licenses array to a list, making it easier to remove items from
            List<string> hasLicensesList = licenses.OfType<string>().ToList();

            //Removes all incorrect falses from the list
            for  (int i = 0; i < hasLicensesList.Count; i++)
            {
                if (hasLicensesList[i] == "true")
                {
                    hasLicensesList.RemoveAt(i + 1);
                }
            }

            string civLicenseString = "\"[";
            string copLicenseString = "\"[";
            string medicLicenseString = "\"[";

            List<license> civLicenseList = new List<license>();
            civLicenseList = convertLicenses(civLicenses);

            List<license> copLicenseList = new List<license>();
            copLicenseList = convertLicenses(copLicenses);

            List<license> medicLicenseList = new List<license>();
            medicLicenseList = convertLicenses(medicLicenses);

            //Civ licenses
            for (int i = 0; i < civLicenseList.Count; i++)
            {
                //If the user has that license
                if (hasLicensesList[i] == "true")
                {
                    if (i == civLicenseList.Count - 1)
                    {
                        civLicenseString += "[`" + licenseList[i].licenseName + "`,1]";
                    }
                    else
                    {
                        civLicenseString += "[`" + licenseList[i].licenseName + "`,1],";
                    }

                }
                else
                {
                    if (i == civLicenseList.Count - 1)
                    {
                        civLicenseString += "[`" + licenseList[i].licenseName + "`,0]";
                    }
                    else
                    {
                        civLicenseString += "[`" + licenseList[i].licenseName + "`,0],";
                    }
                }
            }

            //Cop licenses
            for (int i = civLicenseList.Count; i < civLicenseList.Count + copLicenseList.Count; i++)
            {
                //If the user has that license
                if (hasLicensesList[i] == "true")
                {
                    if (i == civLicenseList.Count + copLicenseList.Count - 1)
                    {
                        copLicenseString += "[`" + licenseList[i].licenseName + "`,1]";
                    }
                    else
                    {
                        copLicenseString += "[`" + licenseList[i].licenseName + "`,1],";
                    }

                }
                else
                {
                    if (i == civLicenseList.Count + copLicenseList.Count - 1)
                    {
                        copLicenseString += "[`" + licenseList[i].licenseName + "`,0]";
                    }
                    else
                    {
                        copLicenseString += "[`" + licenseList[i].licenseName + "`,0],";
                    }
                }
            }

            //Medic licenses
            for (int i = civLicenseList.Count + copLicenseList.Count; i < licenseList.Count; i++)
            {
                //If the user has that license
                if (hasLicensesList[i] == "true")
                {
                    if (i == hasLicensesList.Count - 1)
                    {
                        medicLicenseString += "[`" + licenseList[i].licenseName + "`,1]";
                    }
                    else
                    {
                        medicLicenseString += "[`" + licenseList[i].licenseName + "`,1],";
                    }

                }
                else
                {
                    if (i == hasLicensesList.Count - 1)
                    {
                        medicLicenseString += "[`" + licenseList[i].licenseName + "`,0]";
                    }
                    else
                    {
                        medicLicenseString += "[`" + licenseList[i].licenseName + "`,0],";
                    }
                }
            }

            civLicenseString += "]\"";
            copLicenseString += "]\"";
            medicLicenseString += "]\"";

            //Connects to MySQL database and executes the update query for all the licenses
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "UPDATE players SET civ_licenses=@civLicenseString, cop_licenses=@copLicenseString, med_licenses=@medicLicenseString WHERE uid=@uid";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@civLicenseString", civLicenseString);
                cmd.Parameters.AddWithValue("@copLicenseString", copLicenseString);
                cmd.Parameters.AddWithValue("@medicLicenseString", medicLicenseString);
                cmd.Parameters.AddWithValue("@uid", uid);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                System.Diagnostics.Debug.WriteLine("Update successfull!");

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();

        }

        public string getVehicles()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string sql;
                sql = "SELECT * FROM vehicles WHERE pid=@pid";

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@pid", pid);
                MySqlDataReader reader = cmd.ExecuteReader();

                string HtmlString = "";
                int counter = 0;

                while (reader.Read() && counter < vehicleCount)
                {
                    counter++;
                    int id = reader.GetInt32(0);
                    string side = reader.GetString(1);
                    string className = reader.GetString(2);
                    string type = reader.GetString(3);
                    long pid = reader.GetInt64(4);
                    int alive = reader.GetInt32(5);
                    int blacklist = reader.GetInt32(6);
                    int active = reader.GetInt32(7);

                    HtmlString += "<tr><td>" + id + "</td><td>" + side + "</td>" +
                        "<td>" + className + "</td><td>" + type + "</td><td>" + alive + "</td><td>" + blacklist +
                        "</td><td>" + active + "</td><td>" + pid + "</td><td> <a href='/Vehicles/EditVehicle?id=" + id + "'>Edit</a> </td></tr>";
                }
                reader.Close();
                return HtmlString;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();
            return "";
        }

        public string getHouses()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string sql;
                sql = "SELECT * FROM houses WHERE pid = '" + pid + "'";

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                string HtmlString = "";
                int counter = 0;

                while (reader.Read() && counter < houseCount)
                {
                    counter++;
                    int id = reader.GetInt32(0);
                    long pid = reader.GetInt64(1);
                    int owned = reader.GetInt32(3);
                    int garage = reader.GetInt32(4);
                    string insertTime = reader.GetString(5);

                    HtmlString += "<tr><td>" + id + "</td><td>" + pid + "</td>" +
                        "<td>" + owned + "</td><td>" + garage + "</td><td>" + insertTime + "</td></tr>";
                }

                reader.Close();
                return HtmlString;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();
            return "";
        }

        public void AddWarningPoints()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            if (!Permissions.hasPermission(HttpContext.Current.Session["accessLevel"].ToString(), Permissions.perms.AddWarningPoints) || warningPointsToAdd > 30 || warningPointsToAdd < 0)
            {
                return;
            }

            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "UPDATE players SET warning_points=@warning_points WHERE uid=@uid";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@warning_points", warningPoints + warningPointsToAdd);
                cmd.Parameters.AddWithValue("@uid", uid);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();
        }

    }

    public class license
    {
        public string licenseName;
        public int hasLicense;

        public license(string a, int b)
        {
            this.licenseName = a;
            this.hasLicense = b;
        }
    }
}