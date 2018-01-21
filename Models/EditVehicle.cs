using AdminPanel.Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AdminPanel.Models
{
    public class EditVehicle
    {
        public int id { get; set; }
        public string side { get; set; }
        public string className { get; set; }
        public string type { get; set; }
        public int alive { get; set; }
        public int blacklist { get; set; }
        public int active { get; set; }
        public long pid { get; set; }
        public int plate { get; set; }
        public int color { get; set; }
        public string inventory { get; set; }
        public string gear { get; set; }
        public string insertTime { get; set; }
        public string ownerName { get; set; }

        public void setOwnerName()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT name FROM players WHERE pid='" + pid + "'";
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ownerName = reader.GetString(0);
                }
                cmd.Dispose();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();
        }

        public void setStats()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT * FROM vehicles WHERE id='" + id + "'";
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    side = reader.GetString(1);
                    className = reader.GetString(2);
                    type = reader.GetString(3);
                    pid = reader.GetInt64(4);
                    alive = reader.GetInt32(5);
                    blacklist = reader.GetInt32(6);
                    active = reader.GetInt32(7);
                    plate = reader.GetInt32(8);
                    color = reader.GetInt32(9);
                    inventory = reader.GetString(10);
                    gear = reader.GetString(11);
                    insertTime = reader.GetString(14);
                }
                cmd.Dispose();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();

            setOwnerName();
        }

        public void updateStats()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            Database database = new Database();

            if (!Permissions.hasPermission(HttpContext.Current.Session["accessLevel"].ToString(), Permissions.perms.EditVehicleAlive))
            {
                alive = Convert.ToInt32(database.getDatabaseValue("vehicles", "alive", id));
            }
            if (!Permissions.hasPermission(HttpContext.Current.Session["accessLevel"].ToString(), Permissions.perms.EditVehicleBlacklist))
            {
                blacklist = Convert.ToInt32(database.getDatabaseValue("vehicles", "blacklist", id));
            }
            if (!Permissions.hasPermission(HttpContext.Current.Session["accessLevel"].ToString(), Permissions.perms.EditVehicleActive))
            {
                active = Convert.ToInt32(database.getDatabaseValue("vehicles", "active", id));
            }
            if (!Permissions.hasPermission(HttpContext.Current.Session["accessLevel"].ToString(), Permissions.perms.EditVehicleColour))
            {
                color = Convert.ToInt32(database.getDatabaseValue("vehicles", "color", id));
            }

            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "UPDATE vehicles SET alive=@alive, blacklist=@blacklist, active=@active, color=@color WHERE id=@id";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@alive", alive);
                cmd.Parameters.AddWithValue("@blacklist", blacklist);
                cmd.Parameters.AddWithValue("@active", active);
                cmd.Parameters.AddWithValue("@color", color);
                cmd.Parameters.AddWithValue("@id", id);
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