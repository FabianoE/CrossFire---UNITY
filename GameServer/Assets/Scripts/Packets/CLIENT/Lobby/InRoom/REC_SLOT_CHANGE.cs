using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.CLIENT.Lobby
{
    class REC_SLOT_CHANGE
    {
        public static void Received(Packet packet, Player player)
        {
            int slotid = packet.ReadInt();

            if (player.room.slots[slotid].player != null || player.room.GetCountPlayersByTeam(slotid % 2) >= player.room.maxPlayer / 2 || (slotid % 2) == (player.Team))
                return;

            slotid = player.room.GetSlotNullByTeam(slotid % 2);

            player.room.SendData(new GameServer.Packets.SERVER.Lobby.InRoom.SEND_SLOT_CHANGE(player.room.slots[player.slotId], player, true));
            player.room.slots[player.slotId].Clear();
            player.slotId = slotid;
            ///
            var slot = player.room.slots[slotid];
            slot.player = player;
            slot.exp = 0;
            slot.gold = 0;
            slot.slotStatus = Enums.SlotStatusEnum.NORMAL;
            player.slotId = slot.id;
            player.Team = slot.id % 2;
            ///
            player.room.SendData(new GameServer.Packets.SERVER.Lobby.InRoom.SEND_SLOT_CHANGE(player.room.slots[slotid], player));
        }
    }
}
