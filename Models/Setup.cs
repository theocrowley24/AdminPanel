using AdminPanel.Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace AdminPanel.Models
{
    public class Setup
    {
        public string username { get; set; }
        public string password { get; set; }
        public string dbUsername { get; set; }
        public string dbPassword { get; set; }
        public string dbName { get; set; }
        public string dbAddress { get; set; }
        public string dbPort { get; set; }
        public string communityName { get; set; }

        public bool failed { get; set; }

        public void runSetup()
        {
            if (username == null || password == null || dbUsername == null || dbPassword == null || dbName == null || dbAddress == null || dbPort == null || communityName == null)
            {
                failed = true;
            } else
            {
                failed = false;

                //Saves the community name to web config
                var objConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                var objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");               
                objAppsettings.Settings["communityName"].Value = communityName;
                objConfig.Save();
                

                //Saves database connection string to web config
                var configuration = WebConfigurationManager.OpenWebConfiguration("~");
                var section = (ConnectionStringsSection)configuration.GetSection("connectionStrings");
                section.ConnectionStrings["MySQLConnection-altislife"].ConnectionString = "Data Source=" + dbAddress + ";port=" + dbPort + ";Initial Catalog=" + dbName + ";User Id=" + dbUsername + ";password=" + dbPassword + "";
                configuration.Save();
                
                //Create DB tables
                string connectionString = "Data Source=" + dbAddress + ";port=" + dbPort + ";Initial Catalog=" + dbName + ";User Id=" + dbUsername + ";password=" + dbPassword + "";
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

                    cmd.CommandText = "INSERT INTO users_panel (id, username, password, access_level, banned) VALUES (DEFAULT, @newStaffUsername, @newStaffPassword, @newStaffAccessLevel, DEFAULT)";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@newStaffUsername", username);
                    cmd.Parameters.AddWithValue("@newStaffPassword", SecurePasswordHasher.Hash(password));
                    cmd.Parameters.AddWithValue("@newStaffAccessLevel", "root");
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    string permsString = "";
                    foreach (Permissions.perms perm in Enum.GetValues(typeof(Permissions.perms)))
                    {
                        permsString += perm.ToString() + ",";
                    }
                    permsString = permsString.Remove(permsString.Length - 1);

                    cmd.CommandText = "INSERT INTO ranks_panel (id, name, perms) VALUES (DEFAULT, @name, @perms)";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@name", "root");
                    cmd.Parameters.AddWithValue("@perms", permsString);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    

                }
                catch (Exception e)
                {
                    failed = true;
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

                connection.Close();
            }            
        }
    }
}