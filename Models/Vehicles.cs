using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AdminPanel.Models
{
    public class Vehicles
    {
        public string vehicleSearchPID;
        public int vehicleCount;

        public string getVehicles()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string sql;
                MySqlCommand cmd;
                if (vehicleSearchPID == null || vehicleSearchPID == "")
                {
                    sql = "SELECT * FROM vehicles";
                    cmd = new MySqlCommand(sql, connection);
                }
                else
                {
                    sql = "SELECT * FROM vehicles WHERE pid=@vehicleSearchPID";
                    cmd = new MySqlCommand(sql, connection);
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@vehicleSearchPID", vehicleSearchPID);
                }

                
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
                        "</td><td>" + active + "</td><td>" + pid + "</td><td> <a href='Vehicles/EditVehicle?id=" + id + "'>Edit</a> </td></tr>";
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