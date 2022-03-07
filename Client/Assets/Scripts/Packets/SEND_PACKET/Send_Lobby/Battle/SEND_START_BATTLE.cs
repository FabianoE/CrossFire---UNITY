using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SEND_START_BATTLE
{
    public SEND_START_BATTLE()
    {
        using(Packet packet = new Packet((int)ClientPackets.lobbyData))
        {
            packet.Write((int)LobbyEnums.SubLobbyData.StartBattle);
            packet.Write(100);
            packet.WriteLength();

            Client.instance.tcp.SendData(packet);
        }
    }
}
