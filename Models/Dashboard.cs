using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AdminPanel.Models
{
    public class Dashboard
    {
        public int totalMoney;
        public int totalPlayers;
        public int totalVehicles;

        public void setServerStats()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlDataReader reader;
                string sql;
                sql = "SELECT * FROM players";

                MySqlCommand cmd;

                cmd = new MySqlCommand(sql, connection);
                reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    totalPlayers++;
                }

                reader.Close();

                sql = "SELECT * FROM vehicles";
                cmd = new MySqlCommand(sql, connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    totalVehicles++;
                }

                reader.Close();

                sql = "SELECT cash, bankacc FROM players";
                cmd = new MySqlCommand(sql, connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    totalMoney += reader.GetInt32(0) + reader.GetInt32(1);

                }

                reader.Close();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();
        }
    }
}