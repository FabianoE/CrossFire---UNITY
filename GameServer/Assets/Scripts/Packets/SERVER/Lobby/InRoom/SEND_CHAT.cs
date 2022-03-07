using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.SERVER.Lobby.InRoom
{
    class SEND_CHAT : Packet
    {
        public SEND_CHAT(Player player, string msg, bool inRoom = false)
        {
            string username = player == null ? "SYSTEM" : player.username;

            Write((int)ServerPackets.lobbyData);
            Write((int)Enums.SubLobbyData.Chat);

            int LobbyBattle = 1;

            if (inRoom)
                 LobbyBattle = username != "SYSTEM" ? player.room.slots[player.slotId].slotStatus == Enums.SlotStatusEnum.BATTLE_READY ? 2 : 1 : 1;


            Write(LobbyBattle); //Types: 1 = In Lobby, 2 = In Battle
            Write(1);
            Write(username);
            Write(msg);

            WriteLength();
        }
    }
}
