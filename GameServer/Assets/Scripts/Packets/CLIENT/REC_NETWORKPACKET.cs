using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.CLIENT
{
    class REC_NETWORKPACKET : Structure.PacketHandle
    {
        public override void Handler()
        {
            int latency = _packet.ReadInt();

            _player.ping = latency.ToString();
            CheatDetection.CheckSpeed(_player, DateTime.Now);
        }
    }
}
