using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEND_AUTH_LOGIN : MonoBehaviour
{
    public static void SendLogin(string login, string password)
    {
        using(Packet packet = new Packet((int)ClientPackets.authData))
        {
            packet.Write((int)LobbyEnums.PacketAuth.Login);
            packet.Write(login);
            packet.Write(password);
            packet.WriteLength();

            Client.instance.tcp.SendData(packet);
        }
    }
}
