using AdminPanel.Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AdminPanel.Models
{
    public class EditStaff
    {
        public int id { get; set; }
        public int banned { get; set; }
        public string username { get; set; }
        public string accessLevel { get; set; }
        public string password { get; set; }

        public void setInfo()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string sql = "SELECT * FROM users_panel WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    //Gets all the player stats from the database
                    username = reader.GetString(1);
                    accessLevel = reader.GetString(3);
                    banned = reader.GetInt32(4);
                }

                reader.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();
        }

        public void updateInfo()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;

                //If the password is null then dont update the password!
                if (password == null)
                {
                    cmd.CommandText = "UPDATE users_panel SET username=@username, access_level=@accessLevel, banned=@banned WHERE id=@id";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@accessLevel", accessLevel);
                    cmd.Parameters.AddWithValue("@banned", banned);
                    cmd.Parameters.AddWithValue("@id", id);
                } else
                {
                    cmd.CommandText = "UPDATE users_panel SET username=@username, access_level=@accessLevel, banned=@banned, password=@password WHERE id=@id";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@accessLevel", accessLevel);
                    cmd.Parameters.AddWithValue("@password", SecurePasswordHasher.Hash(password));
                    cmd.Parameters.AddWithValue("@banned", banned);
                    cmd.Parameters.AddWithValue("@id", id);
                }
                
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