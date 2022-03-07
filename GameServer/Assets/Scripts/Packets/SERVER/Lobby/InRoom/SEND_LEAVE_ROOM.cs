using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.SERVER.Lobby.InRoom
{
    class SEND_LEAVE_ROOM : Packet
    {
        public SEND_LEAVE_ROOM(int slotid, int playerid)
        {
            Write((int)ServerPackets.lobbyData);
            Write((int)Enums.SubLobbyData.LeaveRoom);
            Write(slotid);
            Write(playerid);

            WriteLength();
        }
    }
}
