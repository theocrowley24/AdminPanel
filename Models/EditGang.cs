using AdminPanel.Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AdminPanel.Models
{
    public class EditGang
    {
        public int id { get; set; }
        public long owner { get; set; }
        public string name { get; set; }
        public string members { get; set; }
        public int maxMembers { get; set; }
        public int bank { get; set; }
        public int active { get; set; }
        public string insertTime { get; set; }
        public string ownerName { get; set; }
        public List<long> memberList { get; set; }

        public void setInfo()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string sql = "SELECT * FROM gangs WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    //Gets all the gang stats from the database
                    owner = reader.GetInt64(1);
                    name = reader.GetString(2);
                    members = reader.GetString(3);
                    maxMembers = reader.GetInt32(4);
                    bank = reader.GetInt32(5);
                    active = reader.GetInt32(6);
                    insertTime = reader.GetString(7);

                }

                reader.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();

            setOwnerName();
            setMemberNames();
        }

        public void setOwnerName()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT name FROM players WHERE pid=@owner";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@owner", owner);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ownerName = reader.GetString(0);
                }
                cmd.Dispose();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();
        }

        public void setMemberNames()
        {
            memberList = new List<long>();

            for (int i = 0; i < members.Length; i++)
            {
                if (members[i].ToString() == "[" || members[i].ToString() == ",")
                {
                    int j = i + 2;
                    string member = "";
                    while (members[j].ToString() != "`")
                    {
                        member += members[j].ToString();
                        j++;
                    }
                    memberList.Add(Convert.ToInt64(member));
                }
            }

            

        }

        public string getPlayerName(long pid)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            string playerName = "";


            try
            {
                connection.Open();

                string sql = "SELECT name FROM players WHERE pid=@pid";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@pid", pid);
                MySqlDataReader reader = cmd.ExecuteReader();

                

                if (reader.Read())
                {
                    playerName = reader.GetString(0);

                }

                reader.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            connection.Close();

            return playerName;
        }

        public void updateStats()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnection-altislife"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            Database database = new Database();

            if (!Permissions.hasPermission(HttpContext.Current.Session["accessLevel"].ToString(), Permissions.perms.EditGangOwner))
            {
                owner = Convert.ToInt64(database.getDatabaseValue("gangs", "owner", id));
            }
            if (!Permissions.hasPermission(HttpContext.Current.Session["accessLevel"].ToString(), Permissions.perms.EditGangName))
            {
                name = database.getDatabaseValue("gangs", "name", id);
            }
            if (!Permissions.hasPermission(HttpContext.Current.Session["accessLevel"].ToString(), Permissions.perms.EditGangMaxMembers))
            {
                maxMembers = Convert.ToInt32(database.getDatabaseValue("gangs", "maxmembers", id));
            }
            if (!Permissions.hasPermission(HttpContext.Current.Session["accessLevel"].ToString(), Permissions.perms.EditGangBank))
            {
                bank = Convert.ToInt32(database.getDatabaseValue("gangs", "bank", id));
            }
            if (!Permissions.hasPermission(HttpContext.Current.Session["accessLevel"].ToString(), Permissions.perms.EditGangActive))
            {
                active = Convert.ToInt32(database.getDatabaseValue("gangs", "active", id));
            }

            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "UPDATE gangs SET owner=@owner, name=@name, maxmembers=@maxMembers, bank=@bank, active=@active WHERE id=@id";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@owner", owner);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@maxMembers", maxMembers);
                cmd.Parameters.AddWithValue("@bank", bank);
                cmd.Parameters.AddWithValue("@active", active);
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