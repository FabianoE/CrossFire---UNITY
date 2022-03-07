using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.CLIENT
{
    class REC_CHANGE_WEAPON : Structure.PacketHandle
    {
        public override void Handler()
        {
            int weaponid = _packet.ReadInt();

            new Packets.SERVER.SEND_CHANGE_WEAPON(_player, weaponid);
        }
    }
}
