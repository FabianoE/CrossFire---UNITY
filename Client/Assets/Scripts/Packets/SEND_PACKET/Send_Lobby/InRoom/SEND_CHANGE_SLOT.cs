using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEND_CHANGE_SLOT : MonoBehaviour
{
    public static void onSendChangeSlot(int slotid)
    {
        using (Packet packet = new Packet((int)ClientPackets.lobbyData))
        {
            packet.Write((int)LobbyEnums.SubLobbyData.SlotChange);
            packet.Write(slotid);
            packet.WriteLength();
            Client.instance.tcp.SendData(packet);
        }
    }
}
