using AdminPanel.Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AdminPanel.Models
{
    public class Staff
    {
        public string staffSearchName;
        public int staffCount;

        public string newStaffUsername { get; set; }
        public string newStaffPassword { get; set; }
        public string newStaffPasswordRepeat { get; set; }
        public string newStaffAccessLevel { get; set; }

        public string getStaff()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string sql;
                MySqlCommand cmd;
                if (staffSearchName == null || staffSearchName == "")
                {
                    sql = "SELECT * FROM users_panel";
                    cmd = new MySqlCommand(sql, connection);
                }
                else
                {
                    sql = "SELECT * FROM users_panel WHERE username LIKE @staffSearchName";
                    cmd = new MySqlCommand(sql, connection);
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@staffSearchName", staffSearchName + "%");
                }

                
                MySqlDataReader reader = cmd.ExecuteReader();

                string HtmlString = "";
                int counter = 0;

                while (reader.Read() && counter < staffCount)
                {
                    counter++;
                    int id = reader.GetInt32(0);
                    string username = reader.GetString(1);
                    string rank = reader.GetString(3);
                    int banned = reader.GetInt32(4);

                    HtmlString += "<tr><td>" + id + "</td><td>" + username + "</td>" +
                        "<td>" + rank + "</td><td>" + banned + "</td><td> <a href='Staff/EditStaff?id=" + id + "'>Edit</a> </td></tr>";
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

        public void createNewStaff()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "INSERT INTO users_panel (id, username, password, access_level, banned) VALUES (DEFAULT, @newStaffUsername, @newStaffPassword, @newStaffAccessLevel, DEFAULT)";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@newStaffUsername", newStaffUsername);
                cmd.Parameters.AddWithValue("@newStaffPassword", SecurePasswordHasher.Hash(newStaffPassword));
                cmd.Parameters.AddWithValue("@newStaffAccessLevel", newStaffAccessLevel);
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