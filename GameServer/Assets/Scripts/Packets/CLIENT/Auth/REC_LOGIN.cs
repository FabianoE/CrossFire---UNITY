using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.CLIENT.Auth
{
    class REC_LOGIN
    {
        public static void REC_LOGIN_DATA(int _fromClient, Packet packet)
        {
            string username = packet.ReadString();
            string password = packet.ReadString();

            string[] returnLogin = Server.connectiondb.AuthLogin(username, password);
            if (returnLogin != null)
            {
                Server.clients[_fromClient].player = new Player(_fromClient, returnLogin);

                Client player = Server.clients[_fromClient];

                player.tcp.SendData(new SERVER.Auth.SEND_LOGIN_DATA(true)); //Send Success in login

                player.tcp.SendData(new SERVER.Lobby.SEND_PLAYER_INFO(player.player)); //Send Player Info

                player.tcp.SendData(new SERVER.Lobby.SEND_INVENTORY_DATA(player.player)); //Send Current Iventory and bags

                player.tcp.SendData(new SERVER.Lobby.SEND_INVENTORY_DATA(player.player, player.player.characterItems)); //Send Characters List

                player.tcp.SendData(new SERVER.Lobby.SEND_ROOM_LIST());
                
            }
            else
            {
                Server.clients[_fromClient].tcp.SendData(new SERVER.Auth.SEND_LOGIN_DATA(false));
                UnityEngine.Debug.LogError("NÃO LOGADO");
            }
        }
    }
}
