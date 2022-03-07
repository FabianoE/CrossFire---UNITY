using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.CLIENT.Lobby
{
    class REC_CREATE_ROOM
    {
        public static void Received(Packet packet, Player player)
        {
            string roomName = packet.ReadString();
            int roomMode = packet.ReadInt();
            int roomMap = packet.ReadInt();
            int roomType = packet.ReadInt();
            int roomTypeOption = packet.ReadInt();
            int roomPlayer = packet.ReadInt();
            int roomRespawnTime = packet.ReadInt();

            UnityEngine.Debug.LogWarning("Sala");

            int response = Game.RoomList.CreateRoom(roomName, roomMode, roomMap, roomType, roomTypeOption, roomRespawnTime, roomPlayer, player.id);

            if (response != -1)
            {
                UnityEngine.Debug.LogWarning("Sala criada: " + response + ":: QNT" + roomPlayer);

                Room room = Game.RoomList.rooms[response];
                room.AddPlayer(Server.clients[player.id]);
                room.SetRoomMode();
                Game.RoomList.SendListAllPlayers();
            }
        }
    }
}
