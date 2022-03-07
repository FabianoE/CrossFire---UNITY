using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GameServer.Packets.CLIENT
{
    class REC_MOVEMENT : Structure.PacketHandle
    {
        public override void Handler()
        {
            try
            {
                if (_player == null)
                    return;

                Vector3 pos = _packet.ReadVector3();
                Quaternion rot = _packet.ReadQuaternion();

                _player.SetPosition(pos, rot);

                using (Packet packet = new Packet((int)ServerPackets.playerPosition))
                {
                    packet.Write(_player.id);
                    packet.Write(pos);
                    packet.Write(rot);

                    ServerSend.SendUDPDataToAll(packet);
                }
            }
            catch { }
        }
    }
}
