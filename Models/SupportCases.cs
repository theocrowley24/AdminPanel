using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using static AdminPanel.Helpers.SupportCases;

namespace AdminPanel.Models
{
    public class SupportCases
    {
        public string user { get; set; }
        public supportTypes supportType { get; set; }
        public string description { get; set; }
        public string time { get; set; }

        public string caseSearch { get; set; }
        public int caseCount { get; set; }

        public void addNewCase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "INSERT INTO support_cases_panel (id, staff_username, player_username, description, type, open, time) VALUES (DEFAULT, @staffUsername, @playerUsername, @description, @type, DEFAULT, @time)";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@staffUsername", HttpContext.Current.Session["username"]);
                cmd.Parameters.AddWithValue("@playerUsername", user);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@type", supportType.ToString());
                cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString());
                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();
        }

        public string getStaff()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string sql;
                MySqlCommand cmd;
                if (caseSearch == null || caseSearch == "")
                {
                    sql = "SELECT * FROM support_cases_panel";
                    cmd = new MySqlCommand(sql, connection);
                }
                else
                {
                    sql = "SELECT * FROM support_cases_panel WHERE staff_username LIKE @caseSearch";
                    cmd = new MySqlCommand(sql, connection);
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@caseSearch", caseSearch + "%");
                }


                MySqlDataReader reader = cmd.ExecuteReader();

                string HtmlString = "";
                int counter = 0;

                while (reader.Read() && counter < caseCount)
                {
                    counter++;
                    int id = reader.GetInt32(0);
                    string staffUsername = reader.GetString(1);
                    string playerUsername = reader.GetString(2);
                    string description = reader.GetString(3);
                    string type = reader.GetString(4);
                    int open = reader.GetInt32(5);
                    string time = reader.GetString(6);

                    string openString;

                    if (open == 1)
                    {
                        openString = "Yes";
                    } else
                    {
                        openString = "No";
                    }

                    HtmlString += "<tr><td>" + id + "</td><td>" + staffUsername + "</td>" +
                        "<td>" + playerUsername + "</td><td>" + type + "</td><td>" + openString + "</td><td>" + time + "</td><td> <a href='SupportCases/EditCase?id=" + id + "'>Edit</a> </td></tr>";
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
    }
}