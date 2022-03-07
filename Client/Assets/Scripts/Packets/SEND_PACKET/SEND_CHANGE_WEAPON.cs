using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEND_CHANGE_WEAPON : MonoBehaviour
{

    public static void CHANGE_WEAPON(int id)
    {
        using (Packet packet = new Packet((int)ClientPackets.playerChangeWeapon))
        {
            packet.Write(id);
            packet.WriteLength();

            Client.instance.tcp.SendData(packet);
        }
    }
}
