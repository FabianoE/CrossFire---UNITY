using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.CLIENT.Lobby
{
    class REC_CHAT
    {
        public static void Received(Packet packet, Player player)
        {
            try
            {
                int msgType = packet.ReadInt();
                string msg = packet.ReadString();

                if(msg == "AttData")
                {
                    player.inventoryItems = Server.connectiondb.GetInventoryItems(player.dbid); //ATT INVENTORY LIST
                    player.client.tcp.SendData(new SERVER.Lobby.SEND_PLAYER_INFO(player)); //SEND USER INFO -- INCOMPLETE, NO GET ATUAL DATA
                    player.client.tcp.SendData(new SERVER.Lobby.SEND_INVENTORY_DATA(player)); //SEND INVENTORY

                    return;
                }

                if (player.room != null)
                {
                    if (msg == "CloseBattle")
                    {
                        player.room.StopBattle();
                        return;
                    }
                    else if (msg == "GetAWM")
                    {
                        player.primaryweaponid = "3";
                        player.AddWeapon(3);
                        new Packets.SERVER.SEND_CHANGE_WEAPON(player, 1);
                        return;
                    }
                    else if (msg == "GetAK")
                    {
                        player.primaryweaponid = "1";
                        player.AddWeapon(1);
                        new Packets.SERVER.SEND_CHANGE_WEAPON(player, 1);
                        return;
                    }
                    player.room.SendData(new SERVER.Lobby.InRoom.SEND_CHAT(player, msg, true));
                }
                else
                {
                    foreach(var pl in Server.clients.Values)
                    {
                        if(pl.player == null || pl.player.room == null)
                        {
                            pl.tcp.SendData(new SERVER.Lobby.InRoom.SEND_CHAT(player, msg, false));
                        }
                    }
                }
            }
            catch { }
        }
    }
}
