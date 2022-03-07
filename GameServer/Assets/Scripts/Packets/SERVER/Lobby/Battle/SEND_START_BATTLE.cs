using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.SERVER.Lobby.Battle
{
    class SEND_START_BATTLE : Packet
    {
        public SEND_START_BATTLE()
        {
            Write((int)ServerPackets.lobbyData);
            Write((int)Enums.SubLobbyData.StartBattle);

            WriteLength();
        }
    }
}
