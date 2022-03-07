using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class REC_LEAVE_ROOM : MonoBehaviour
{
    public static void Received(Packet packet)
    {
        int slotid = packet.ReadInt();
        int playerid = packet.ReadInt();

        if(playerid == Client.instance.myId)
        {
            for(int i = 0; i < LobbyItemsObject.instance.RoomAllSlots.Length; i++)
            {
                LobbyItemsObject.instance.RoomAllSlots[i].isNull = true;
            }

            LobbyItemsObject.instance.currentBox = LobbyEnums.LobbyBoxEnum.Lobby;
            LobbyItemsObject.instance.SetBox(LobbyEnums.LobbyBoxEnum.Lobby);
        }

        LobbyItemsObject.instance.RoomAllSlots[slotid].isNull = true;
    }

}
