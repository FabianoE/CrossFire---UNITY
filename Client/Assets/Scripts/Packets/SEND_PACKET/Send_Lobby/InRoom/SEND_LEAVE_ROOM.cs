using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


 class SEND_LEAVE_ROOM
{
    public SEND_LEAVE_ROOM()
    {
        using(Packet packet = new Packet((int)ClientPackets.lobbyData))
        {
            packet.Write((int)LobbyEnums.SubLobbyData.LeaveRoom);
            packet.Write("");
            packet.WriteLength();

            Client.instance.tcp.SendData(packet);
        }
    }
}
