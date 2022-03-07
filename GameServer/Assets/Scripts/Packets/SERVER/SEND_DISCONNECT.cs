using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.SERVER
{
    class SEND_DISCONNECT
    {
       public SEND_DISCONNECT(int _fromClient)
       {
            using (Packet packet = new Packet((int)ServerPackets.playerDisconnected))
            {
                packet.Write(_fromClient);

                SendPacket(packet);
            }
       }

      private void SendPacket(Packet _packet)
      {
            foreach(Client client in Server.clients.Values)
            {
                ServerSend.SendTCPData(client.id ,_packet);
            }
      }

    }
}
