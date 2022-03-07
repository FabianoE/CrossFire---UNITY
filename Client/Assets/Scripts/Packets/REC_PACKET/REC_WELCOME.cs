using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class REC_WELCOME : PacketHandle
{
    public override void Handler()
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;

        LobbyItemsObject.instance.btn_Login.interactable = true;

        //ClientSend.WelcomeReceived();

        //Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }
}
