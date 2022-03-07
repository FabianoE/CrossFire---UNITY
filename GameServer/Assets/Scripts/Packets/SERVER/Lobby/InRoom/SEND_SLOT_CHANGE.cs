using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.SERVER.Lobby.InRoom
{
    class SEND_SLOT_CHANGE : Packet
    {
        public SEND_SLOT_CHANGE(Slot slot)
        {
            Write((int)ServerPackets.lobbyData);
            Write((int)Enums.SubLobbyData.SlotChange);
            Write(1);
            Write(slot.id);
            Write((int)slot.slotStatus);
            WriteLength();
        }

        public SEND_SLOT_CHANGE(Slot slot, Player player, bool remove = false)
        {
            Write((int)ServerPackets.lobbyData);
            Write((int)Enums.SubLobbyData.SlotChange);
            Write(2);
            Write(slot.id);
            Write(remove == true ? 0 : 1);
            if (remove == false)
            {
                byte[] slotData = new SlotRoom
                {
                    host = player.room.hostid.Equals(player.id),
                    slotId = slot.id,
                    playerid = player.id,
                    playerExp = player.exp,
                    playername = player.username,
                    slotStatus = (int)Enums.SlotStatusEnum.NORMAL
                }.Serialize();

                Write(slotData.Length);
                Write(slotData);
            }

            WriteLength();
        }
    }
}
