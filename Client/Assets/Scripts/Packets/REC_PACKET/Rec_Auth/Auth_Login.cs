using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auth_Login : MonoBehaviour
{
    public static void LoginReceived(Packet packet)
    {
        bool result = packet.ReadBool();

        if (result)
        {
            LobbyItemsObject.instance.box_login.SetActive(false);

            Client.instance.udp.Connect(((System.Net.IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        }
        else
        {
            DialogMessage.instance.SetMessage("ERROR IN LOGIN", 1);
            LobbyItemsObject.instance.btn_Login.interactable = true;
        }
    }
}
