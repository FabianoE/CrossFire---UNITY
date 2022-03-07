using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEND_NETWORKDATA
{
    public static void SendLatency()
    {
        using(Packet packet = new Packet((int)ClientPackets.networkData))
        {
            Debug.LogError("PING");

            packet.Write(Client.instance.tcp.lastPing);
            packet.WriteLength();

            Client.instance.tcp.SendData(packet);
        }
    }
}
