using AdminPanel.Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace AdminPanel.Models
{
    public class Login
    {
        public bool verified;
        public string errorMessage;

        public Login(string username, string password)
        {
            verified = false;
            errorMessage = "Unknown error";
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string sql = "SELECT * FROM users_panel WHERE username=@username";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@username", username);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    //Get username and password from the database
                    string dbUsername = reader.GetString(1);
                    string dbPassword = reader.GetString(2);
                    string dbAccessLevel = reader.GetString(3);
                    int dbBanned = reader.GetInt32(4);
          
                    //Check if the password is correct
                    if (SecurePasswordHasher.Verify(password, dbPassword) && dbBanned == 0)
                    {
                        verified = true;
                        HttpContext.Current.Session["username"] = dbUsername;
                        HttpContext.Current.Session["accessLevel"] = dbAccessLevel;
                    } else
                    {
                        verified = false;
                        errorMessage = "Incorrect password";
                    }
                } else
                {
                    verified = false;
                    errorMessage = "Unknown username";
                }
              
                reader.Close();
            } catch (Exception e)
            {
                errorMessage = "Error connecting to database";
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();
        }   
    }

}