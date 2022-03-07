using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.CLIENT
{
    class REC_RESPAWN : Structure.PacketHandle
    {
        public override void Handler()
        {
            new Packets.SERVER.SEND_RESPAWN(_player);
        }
    }
}
