using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.SERVER.Lobby
{
    class SEND_ROOM_LIST : Packet
    {
        public SEND_ROOM_LIST()
        {
            List<RoomListData> list = new List<RoomListData>();

            foreach(Room room in Game.RoomList.rooms.Values)
            {
                if(room.users.Count > 0)
                    list.Add(new RoomListData { 
                    Room_Id = room.id, 
                    Room_Map = room.map, 
                    Room_Mode = (int)room.mode,
                    Room_Name = room.name, 
                    Room_Players = room.users.Count.ToString() + "/" + room.maxPlayer 
                });
                UnityEngine.Debug.LogWarning("MAX PLAYER::" + room.maxPlayer);
            }

            byte[] bt = list.Serialize();

            Write((int)ServerPackets.lobbyData);
            Write((int)Enums.SubLobbyData.RoomList);
            Write(bt.Length);
            Write(bt);
            WriteLength();
        }
    }
}
