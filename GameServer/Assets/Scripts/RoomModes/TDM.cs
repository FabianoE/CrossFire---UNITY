using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.RoomModes
{
    public class TDM 
    {
        ~TDM()
        {
            GC.Collect();
        } 

        Room room = null;
        public TDM(Room room)
        {
            this.room = room;
        }
        public void Update()
        {
            if (room != null && room.GetTimeLeft() <= 0 || (room.type == Enums.RoomTypes.Kill && room.blkills == 10 || room.grkills == 10))
            {
                room.StopBattle();

                string winner = room.blkills > room.grkills ? "BL" : "GR";
                room.SendData(new Packets.SERVER.Lobby.InRoom.SEND_CHAT(null, string.Format("O time {0} Venceu", winner)));
            }
        }
    }
}
