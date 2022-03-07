using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.SERVER
{
    public class SEND_KILLMARK : Packet
    {
        public SEND_KILLMARK(Client player, int type)
        {
            Write((int)ServerPackets.playerDamage);
            Write(3);
            Write(type);
            Write(player.player.LastKills);
            WriteLength();
        }
    }
}
