using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Game
{
    public class RoomList
    {
        public static ConcurrentDictionary<int, Room> rooms = new ConcurrentDictionary<int, Room>();

        public static int CreateRoom(string roomName, int roomMode, int roomMap, int roomType, int roomTypeOption, int roomRespawnTime, int maxPlayer, int hostid)
        {
            using (Room room = new Room())
            {
                room.id = rooms.Count + 1;
                room.name = roomName;
                room.mode = (Enums.RoomMode)roomMode;
                room.map = roomMap;
                room.type = (Enums.RoomTypes)roomType;
                room.typeoption = roomTypeOption;

                if ((Enums.RoomTypes)roomType == Enums.RoomTypes.Kill)
                {
                    switch (roomTypeOption)
                    {
                        case 0:
                            room.RoomObjective = 60;
                            break;
                        case 1:
                            room.RoomObjective = 100;
                            break;
                    }
                }
                else if ((Enums.RoomTypes)roomType == Enums.RoomTypes.Time)
                {
                    switch (roomTypeOption)
                    {
                        case 0:
                            room.RoomObjective = 2;
                            break;
                        case 1:
                            room.RoomObjective = 5;
                            break;
                    }
                }

                room.respawnTime = roomRespawnTime;
                room.maxPlayer = (maxPlayer + 1) * 2;
                room.hostid = hostid;

                if (rooms.TryAdd(room.id, room))
                    return room.id;
            }

            return -1;
        }

        public static void RemoveRoom(Room room)
        {
            rooms.TryRemove(room.id, out room);
        }
        public static void SendListAllPlayers()
        {
            foreach (var player in Server.clients.Values)
            {
                if (player.player != null && player.player.room == null)
                {
                    player.tcp.SendData(new Packets.SERVER.Lobby.SEND_ROOM_LIST());
                }
            }
        }
    }
}
