using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AdminPanel.Models
{
    public class EditCase
    {
        public int id { get; set; }
        public string staffUsername { get; set; }
        public string playerName { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public int open { get; set; }
        public string openString { get; set; }
        public string time { get; set; }

        public void setInfo()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string sql = "SELECT * FROM support_cases_panel WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    //Gets all the case info from the database
                    staffUsername = reader.GetString(1);
                    playerName = reader.GetString(2);
                    description = reader.GetString(3);
                    type = reader.GetString(4);
                    open = reader.GetInt32(5);
                    time = reader.GetString(6);

                    if (open == 1)
                    {
                        openString = "Yes";
                    } else
                    {
                        openString = "No";
                    }
                }

                reader.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();
        }

        public void closeCase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "UPDATE support_cases_panel SET open='0' WHERE id=@id";
                cmd.Prepare();
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

        public void openCase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "UPDATE support_cases_panel SET open='1' WHERE id=@id";
                cmd.Prepare();
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

        public void deleteCase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "DELETE FROM support_cases_panel WHERE id=@id";
                cmd.Prepare();
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