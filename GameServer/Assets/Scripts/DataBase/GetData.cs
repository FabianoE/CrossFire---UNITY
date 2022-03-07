using System;
using System.Collections.Generic;
using System.Text;

using UnityNpgsql;

using MySql.Data;
using MySql.Data.MySqlClient;

namespace GameServer
{
    class GetData
    {
        string connec = string.Format("Server={0};Username={1};DataBase={2};Port={3};Password={4}", "localhost", "postgres", "rivals", "5432", "123");

        //string connec = string.Format("Server={0};Username={1};DataBase={2};Port={3};Password={4}", "localhost", "postgres", "rivals", "4556", "4554");
        public bool CheckConnection()
        {
            bool connected = false;
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connec))
                {
                    conn.Open();
                    UnityEngine.Debug.LogError("[->] PGSQL DB::CONNECTED");
                    connected = true;
                    conn.Close();
                }
                AuthLogin("evr", "123"); //Test
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("[->] PGSQL DB::NOT CONNECTED::" + ex.Message);
            }

            return connected;
        }

        public string[] AuthLogin(string login, string password)
        {
            string[] playerData = new string[7];
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connec))
                using (NpgsqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);
                    command.CommandText = $"SELECT * FROM accounts WHERE login=@Login and password=@Password";
                    using (NpgsqlDataReader data = command.ExecuteReader())
                    {
                        data.Read();
                        if (data.HasRows)
                        {
                            playerData[0] = data["id"].ToString();
                            playerData[1] = data["player_name"].ToString();
                            playerData[2] = data["player_cash"].ToString();
                            playerData[3] = data["player_gold"].ToString();
                            playerData[4] = data["player_exp"].ToString();
                            playerData[5] = data["player_kills"].ToString();
                            playerData[6] = data["player_deaths"].ToString();
                        }
                        else
                            return null;

                        data.Close();
                        connection.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                UnityEngine.Debug.LogWarning(ex.Message);
            }

            return playerData;
        }

        /// <summary>
        /// Pega o ID do ITEM no iventario
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bag"></param>
        /// <returns></returns>
        public int GetIdInventoryBag(int id, int bag, string bagtype)
        {
            int primary = 0;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connec))
                using (NpgsqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@ownerId", id);
                    command.CommandText = "SELECT bag1_primary, bag1_secondary, bag1_melee, bag1_grenade FROM accounts WHERE id=@ownerId";
                    using (NpgsqlDataReader data = command.ExecuteReader())
                    {
                        data.Read();
                        if (data.HasRows)
                            primary = (int)data[bagtype];

                        data.Close();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning(ex.Message);
            }
            return primary;
        }

        public List<InventoryItems> GetInventoryItems(int id, string query = "player_inventory")
        {
            List<InventoryItems> items = new List<InventoryItems>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connec))
                using (NpgsqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@ownerId", id);
                    command.CommandText = $"SELECT * FROM " + query + " WHERE player_id=@ownerId";
                    using (NpgsqlDataReader data = command.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            items.Add(new InventoryItems { inventoryID = (int)data["id"], itemID = (int)data["item_id"] });
                        }
                        data.Close();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning("ERROR:" + ex.Message);
            }

            return items;
        }

        public bool UpdateItemDB(string query)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connec))
            using (NpgsqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = query;
                using (NpgsqlDataReader data = command.ExecuteReader())
                {
                    if (data.Read())
                    {
                        connection.Close();
                        data.Close();
                        return true;
                    }
                }
            }

            return false;
        }

        public bool SetExp(int playerid, int exp)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connec))
            using (NpgsqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "UPDATE accounts SET player_exp = player_exp + " + exp + " WHERE id = " + playerid;
                using(NpgsqlDataReader data = command.ExecuteReader())
                {
                    if (data.Read())
                    {
                        connection.Close();
                        data.Close();
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
