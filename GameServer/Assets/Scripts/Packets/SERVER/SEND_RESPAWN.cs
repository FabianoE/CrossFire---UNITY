using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.SERVER
{
    class SEND_RESPAWN
    {
        public SEND_RESPAWN(Player player)
        {
            player.Health = 100;

            using (Packet packet = new Packet((int)ServerPackets.playerRespawn))
            {
                packet.Write(player.id);
                packet.Write(player.Health);
                packet.WriteLength();

                foreach (Client client in Server.clients.Values)
                {
                    client.tcp.SendData(packet);
                }

                player.Alive = true;
            }
        }
    }
}
