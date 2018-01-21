using AdminPanel.Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AdminPanel.Models
{
    public class Settings
    {
        public string rankName { get; set; }
        public string[] permissions {get; set;}
        public List<string> ranks { get; set; }
        public string[] updatedPermissions { get; set; }
        public string rank { get; set; }

        public void createRank()
        {
            List<string> permissionsList = permissions.OfType<string>().ToList();

            //Removes all incorrect falses from the list
            for (int i = 0; i < permissionsList.Count; i++)
            {
                if (permissionsList[i] == "true")
                {
                    permissionsList.RemoveAt(i + 1);
                }
            }

            Array values = Enum.GetValues(typeof(Permissions.perms));
            string permsString = "";

            for (int j = 0; j < permissionsList.Count; j++)
            {
                if (permissionsList[j] == "true")
                {
                    string value = values.GetValue(Convert.ToInt32(j)).ToString();
                    permsString += value + ",";
                }
            }

            permsString = permsString.Remove(permsString.Length - 1);

            //Inserts rank into database
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "INSERT INTO ranks_panel (id, name, perms) VALUES (DEFAULT, @name, @perms)";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@name", rankName);
                cmd.Parameters.AddWithValue("@perms", permsString);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();
        }

        public void getRanks()
        {
            ranks = new List<string>();

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
        }

        public bool hasRank(Permissions.perms perm)
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
                cmd.Parameters.AddWithValue("@name", rank);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    //Gets all the player stats from the database
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
            } else
            {
                return false;
            }
        }

        public void updateRank()
        {
            //Coverts the licenses array to a list, making it easier to remove items from
            List<string> updatedPermissionsList = updatedPermissions.OfType<string>().ToList();

            //Removes all incorrect falses from the list
            for (int i = 0; i < updatedPermissionsList.Count; i++)
            {
                if (updatedPermissionsList[i] == "true")
                {
                    updatedPermissionsList.RemoveAt(i + 1);
                }
            }

            Array values = Enum.GetValues(typeof(Permissions.perms));
            string permsString = "";

            for (int j = 0; j < updatedPermissionsList.Count; j++)
            {
                if (updatedPermissionsList[j] == "true")
                {
                    string value = values.GetValue(Convert.ToInt32(j)).ToString();
                    permsString += value + ",";
                }
            }

            permsString = permsString.Remove(permsString.Length - 1);

            //Inserts rank into database
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "UPDATE ranks_panel SET perms=@perms WHERE name=@name";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@perms", permsString);
                cmd.Parameters.AddWithValue("@name", rank);
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
}