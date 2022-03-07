using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.CLIENT.Lobby
{
    class REC_LEAVE_ROOM
    {
        public static void Received(Packet packet, Player player)
        {
            UnityEngine.Debug.LogWarning($"Player Disconnected on the room {player.username}");
            player.room.Disconnected(Server.clients[player.id]);
        }
    }
}
