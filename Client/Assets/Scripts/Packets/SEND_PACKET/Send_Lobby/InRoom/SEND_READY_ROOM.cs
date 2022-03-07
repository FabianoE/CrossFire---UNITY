using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class SEND_READY_ROOM
{
    public SEND_READY_ROOM()
    {
        using(Packet packet = new Packet((int)ClientPackets.lobbyData))
        {
            packet.Write((int)LobbyEnums.SubLobbyData.ReadyRoom);

            packet.WriteLength();

            Client.instance.tcp.SendData(packet);
        }
    }
}