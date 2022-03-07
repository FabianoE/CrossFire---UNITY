using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REC_CHAT
{
    public static void Received(Packet packet)
    {
        int msgType = packet.ReadInt();
        int chat = packet.ReadInt();
        string playername = packet.ReadString();
        string msg = packet.ReadString();

        Debug.LogError("MESSAGE RECEIVED::" + playername + "::" + msg);
        if (msgType == 1)
            Lobby_ChatController.instance.SpawnMessage(playername, msg);
        else
            Pvp_ChatController.instance.SpawnMessage(playername, msg, chat);
    }
}
