using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AdminPanel.Models
{
    public class Houses
    {
        public string houseSearchPID;
        public int houseCount;

        public string getHouses()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string sql;
                MySqlCommand cmd;
                if (houseSearchPID == null || houseSearchPID == "")
                {
                    sql = "SELECT * FROM houses";
                    cmd = new MySqlCommand(sql, connection);
                }
                else
                {
                    sql = "SELECT * FROM houses WHERE pid=@houseSearchPID";
                    cmd = new MySqlCommand(sql, connection);
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@houseSearchPID", houseSearchPID);
                }

                
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
    }
}