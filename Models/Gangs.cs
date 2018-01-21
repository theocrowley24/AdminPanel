using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AdminPanel.Models
{
    public class Gangs
    {

        public string gangSearchName;
        public int gangCount;

        public string getGangs()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string sql;
                MySqlCommand cmd;
                if (gangSearchName == null || gangSearchName == "")
                {
                    sql = "SELECT * FROM gangs";
                    cmd = new MySqlCommand(sql, connection);
                }
                else
                {
                    sql = "SELECT * FROM gangs WHERE name LIKE @gangSearchName";
                    cmd = new MySqlCommand(sql, connection);
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@gangSearchName", gangSearchName + "%");
                }

                
                MySqlDataReader reader = cmd.ExecuteReader();

                string HtmlString = "";
                int counter = 0;

                while (reader.Read() && counter < gangCount)
                {
                    counter++;
                    int id = reader.GetInt32(0);
                    string owner = reader.GetString(1);
                    string name = reader.GetString(2);
                    string members = reader.GetString(3);
                    int maxMembers = reader.GetInt32(4);
                    int bank = reader.GetInt32(5);
                    int active = reader.GetInt32(6);
                    string insertTime = reader.GetString(7);

                    HtmlString += "<tr><td>" + id + "</td><td>" + owner + "</td>" +
                        "<td>" + name + "</td><td>" + maxMembers + "</td><td>" + bank + "</td><td>" + active + "</td><td>" + insertTime + "</td><td> <a href='Gangs/EditGang?id=" + id + "'>Edit</a> </td></tr>";
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