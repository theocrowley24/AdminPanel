using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AdminPanel.Helpers
{
    public class Database
    {
        //Returns a value from the database, table and columnName should match exactly with the databse
        public string getDatabaseValue(string table, string columnName, int uid)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            string value = "";

            try
            {
                connection.Open();

                string sql = "";

                if (table == "players")
                {
                    sql = "SELECT " + columnName + " FROM " + table + " WHERE uid=@uid";
                } else if (table == "vehicles")
                {
                    sql = "SELECT " + columnName + " FROM " + table + " WHERE id=@uid";
                } else if (table == "gangs")
                {
                    sql = "SELECT " + columnName + " FROM " + table + " WHERE id=@uid";
                } else
                {
                    return "";
                }

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@uid", uid);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    //Gets all the player stats from the database
                    value = reader.GetString(0);
                    //For some reason getting a 1/0 from the database can result in True/False instead?
                    //This fixes it
                    if (value == "False")
                    {
                        value = "0";
                    }
                    if (value == "True")
                    {
                        value = "1";
                    }
                }

                reader.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();

            return value;
        }

    }
}