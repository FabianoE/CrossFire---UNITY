using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.SERVER.Lobby.InRoom
{
    class SEND_ENTER_ROOM : Packet
    {
        public SEND_ENTER_ROOM(Player player, int slot)
        {
            List<SlotRoom> slots = new List<SlotRoom>();

            for(int i = 0; i < player.room.slots.Length; i++)
            {
                if (player.room.slots[i].player != null)
                {
                    slots.Add(new SlotRoom
                    {
                        host = player.room.hostid == player.room.slots[i].player.id ? true : false,
                        slotId = i,
                        playerid = player.room.slots[i].player.id,
                        playername = player.room.slots[i].player.username,
                        slotStatus = (int)player.room.slots[i].slotStatus,
                        playerExp = player.room.slots[i].player.exp,
                    });
                }
            }

            byte[] slotbyte = slots.Serialize();

            Write((int)ServerPackets.lobbyData);
            Write((int)Enums.SubLobbyData.EnterRoom);
            Write(player.room.name);
            Write((int)player.room.mode);
            Write(player.room.map);
            Write(player.room.respawnTime);
            Write((int)player.room.type);
            Write(player.room.RoomObjective);
            //
            Write(slotbyte.Length);
            Write(slotbyte);

            WriteLength();
        }
    }
}

[System.Serializable]
public class SlotRoom
{
    public bool host { get; set; }
    public int slotId { get; set; }
    public int playerid { get; set; }
    public int slotStatus { get; set; }
    public int playerExp { get; set; }
    public string playername { get; set; }
}
