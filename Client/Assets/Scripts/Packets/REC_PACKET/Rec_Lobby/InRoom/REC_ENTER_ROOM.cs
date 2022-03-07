using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class REC_ENTER_ROOM
{
   public static void Received(Packet packet)
   {
        string roomname = packet.ReadString();
        int roommode = packet.ReadInt();
        int roommap = packet.ReadInt();
        int roomrespawntime = packet.ReadInt();
        int roomtype = packet.ReadInt();
        int roomtypeoption = packet.ReadInt();
        int slotsbytelenght = packet.ReadInt();

        List<SlotRoom> slotRoom = packet.ReadBytes(slotsbytelenght).DeserializeObject<List<SlotRoom>>();

        foreach(var obj in slotRoom)
        {
            RoomSlotItem slot = LobbyItemsObject.instance.RoomAllSlots[obj.slotId];

            slot.SetData(obj.playerid, obj.playername, obj.slotStatus, obj.host, obj.playerExp);
            slot.isNull = false;
        }

        LobbyItemsObject.instance.SetBox(LobbyEnums.LobbyBoxEnum.Room);

        LobbyItemsObject.instance.SetRoomData(roomname, roommode, roommap, roomrespawntime, roomtype, roomtypeoption);
   }
}
