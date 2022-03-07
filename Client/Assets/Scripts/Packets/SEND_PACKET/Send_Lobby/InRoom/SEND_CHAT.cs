using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEND_CHAT : MonoBehaviour
{
    public static void OnSendMessage(int msgType, string msg)
    {
        using (Packet packet = new Packet((int)ClientPackets.lobbyData))
        {
            packet.Write((int)LobbyEnums.SubLobbyData.Chat);
            packet.Write(1);
            packet.Write(msg);

            packet.WriteLength();

            Client.instance.tcp.SendData(packet);
        }
    }
}
