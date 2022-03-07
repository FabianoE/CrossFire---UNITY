using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class SEND_CREATE_ROOM : MonoBehaviour
{
    public SEND_CREATE_ROOM(string name, int roomMode, int roomMap, int roomType, int roomTypeOption, int maxPlayers, int respawnTime = 0)
    {
        using (Packet packet = new Packet((int)ClientPackets.lobbyData))
        {
            packet.Write((int)LobbyEnums.SubLobbyData.CreateRoom);
            packet.Write(name);
            packet.Write(roomMode);
            packet.Write(roomMap);
            packet.Write(roomType);
            packet.Write(roomTypeOption);
            packet.Write(maxPlayers);
            packet.Write(respawnTime);
            packet.WriteLength();

            Client.instance.tcp.SendData(packet);
        }
    }
}
