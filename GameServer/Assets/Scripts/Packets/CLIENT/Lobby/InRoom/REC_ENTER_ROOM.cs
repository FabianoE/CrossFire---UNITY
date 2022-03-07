using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.CLIENT.Lobby
{
    class REC_ENTER_ROOM
    {
        public static void Received(Packet packet, Player player)
        {
            int roomid = packet.ReadInt();

            if (Game.RoomList.rooms.ContainsKey(roomid))
            {
                Room room = Game.RoomList.rooms[roomid];
                if(room.users.Count >= room.maxPlayer)
                {
                    Server.clients[player.id].tcp.SendData(new Packets.SERVER.Lobby.InRoom.SEND_CHAT(null, "SALA CHEIA"));
                    return;
                }
                room.AddPlayer(Server.clients[player.id]);
            }
            else
            {
                UnityEngine.Debug.LogWarning("[->] ROOM ID ERROR -> PLAYER ID:" + player.id);
                Server.clients[player.id].tcp.SendData(new Packets.SERVER.Lobby.SEND_ROOM_LIST());
            }
            //new Packets.SERVER.SEND_SPAWN(player.id, player.username);
        }
    }
}
