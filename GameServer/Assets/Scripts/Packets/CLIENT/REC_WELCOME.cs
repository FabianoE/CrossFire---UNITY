using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets
{
    public class REC_WELCOME : Structure.PacketHandle
    {
        public override void Handler()
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            UnityEngine.Debug.LogWarning($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                UnityEngine.Debug.LogWarning($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }

            Server.clients[_fromClient].player = new Player(_fromClient);


            //new Packets.SERVER.SEND_SPAWN(_fromClient, _username);
        }
    }
}
