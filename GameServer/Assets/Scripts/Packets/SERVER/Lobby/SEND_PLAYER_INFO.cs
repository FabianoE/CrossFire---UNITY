using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.SERVER.Lobby
{
    class SEND_PLAYER_INFO : Packet
    {
        public SEND_PLAYER_INFO(Player player)
        {
            Write((int)ServerPackets.lobbyData);
            Write((int)Enums.SubLobbyData.PlayerInfo);
            Write(player.cash);
            Write(player.gold);
            Write(player.exp);
            Write(player.kills);
            Write(player.deaths);
            Write(player.username);


            WriteLength();
        }
    }
}
