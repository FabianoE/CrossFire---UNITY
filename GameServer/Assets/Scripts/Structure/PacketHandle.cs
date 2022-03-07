using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Structure
{
    public class PacketHandle
    {
        public int _fromClient = 0;
        public Player _player = null;
        public Packet _packet = null;

        public void SetPacket(int fromclient, Packet packet)
        {
            _fromClient = fromclient;
            _player = Server.clients[fromclient].player;
            _packet = packet;
        }
    
        public virtual void Handler()
        {
            /* OVERRIDE */
        }
    }
}
