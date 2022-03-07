using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class REC_SLOT_CHANGE : MonoBehaviour
{
    public static void Received(Packet packet)
    {
        int type = packet.ReadInt();

        switch (type)
        {
            case 1:
                try
                {
                    int slotid = packet.ReadInt();
                    int slottype = packet.ReadInt();

                    LobbyItemsObject.instance.RoomAllSlots[slotid].SlotChange(slottype);
                }
                catch { }
                break;
            case 2:
                try
                {
                    int slotid = packet.ReadInt();
                    bool remove = packet.ReadInt() == 0 ? true : false;
                    if (remove)
                    {
                        LobbyItemsObject.instance.RoomAllSlots[slotid].isNull = true;
                        return;
                    }
                    int btlenght = packet.ReadInt();
                    byte[] bt = packet.ReadBytes(btlenght);

                    SlotRoom slotData = bt.DeserializeObject<SlotRoom>();
                    LobbyItemsObject.instance.RoomAllSlots[slotid].SetData(slotData.playerid, slotData.playername, slotData.slotStatus, slotData.host, slotData.playerExp);

                }
                catch { }
                break;
        }
    }
}
