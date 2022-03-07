using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.RoomModes
{
    public class FFA
    {
        ~FFA()
        {
            GC.Collect();
        }

        Room room = null;
        public FFA(Room room)
        {
            this.room = room;
        }
        public void Update()
        {
            if ((room.type == Enums.RoomTypes.Kill && room.ffakills == 10))
            {
                room.StopBattle();
                room.SendData(new Packets.SERVER.Lobby.InRoom.SEND_CHAT(null, "ACE => " + room.GetAcePlayer().username));
            }
        }
    }
}
