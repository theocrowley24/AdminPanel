using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AdminPanel.Models
{
    public class Players
    {

        public string playerSearchName;
        public int playerCount;

        public string getPlayers()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string sql;
                MySqlCommand cmd;

                if (playerSearchName == null || playerSearchName == "")
                {
                    sql = "SELECT * FROM players";
                    cmd = new MySqlCommand(sql, connection);
                } else
                {
                    sql = "SELECT * FROM players WHERE name LIKE @playerSearchName";
                    cmd = new MySqlCommand(sql, connection);
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@playerSearchName", playerSearchName + "%");
                }
                
                
                MySqlDataReader reader = cmd.ExecuteReader();

                string HtmlString = "";
                int counter = 0;

                while (reader.Read() && counter < playerCount)
                {
                    counter++;
                    int uid = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    int cash = reader.GetInt32(4);
                    int bankAccount = reader.GetInt32(5);
                    int copLevel = reader.GetInt32(6);
                    int medicLevel = reader.GetInt32(7);
                    int adminLevel = reader.GetInt32(18);
                    int warningPoints = reader.GetInt32(reader.GetOrdinal("warning_points"));

                    HtmlString += "<tr><td>" + uid + "</td><td>" + name + "</td>" +
                        "<td>" + cash + "</td><td>" + bankAccount + "</td><td>" + copLevel + "</td><td>" + medicLevel + "</td><td>" + adminLevel +
                        "</td><td>" + warningPoints + "</td><td> <a href='Players/EditPlayer?uid=" + uid + "&vehicleCount=5&houseCount=5'>Edit</a> </td></tr>";
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