using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AdminPanel.App_Start
{
    public static class DBSetup
    {
        public static void init()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = connection;
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS `users_panel` (`id` int(11) NOT NULL AUTO_INCREMENT,`username` varchar(45) DEFAULT NULL,`password` varchar(500) DEFAULT NULL,`access_level` varchar(45) DEFAULT NULL,`banned` int(11) DEFAULT '0',PRIMARY KEY(`id`),UNIQUE KEY `username_UNIQUE` (`username`)) ENGINE = InnoDB AUTO_INCREMENT = 10 DEFAULT CHARSET = utf8; ";
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS `support_cases_panel` (`id` int(11) NOT NULL AUTO_INCREMENT,`staff_username` varchar(45) NOT NULL,`player_username` varchar(45) NOT NULL,`description` varchar(500) NOT NULL,`type` varchar(45) NOT NULL,`open` int(11) DEFAULT '1',`time` varchar(45) DEFAULT NULL,PRIMARY KEY(`id`),UNIQUE KEY `id_UNIQUE` (`id`)) ENGINE = InnoDB AUTO_INCREMENT = 3 DEFAULT CHARSET = utf8mb4;";
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS `ranks_panel` (`id` int(11) NOT NULL,`name` varchar(45) NOT NULL,`perms` text NOT NULL,PRIMARY KEY(`id`)) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4;";
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                cmd.CommandText = "ALTER TABLE players ADD COLUMN IF NOT EXISTS warning_points int(10) NOT NULL DEFAULT 0";
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



