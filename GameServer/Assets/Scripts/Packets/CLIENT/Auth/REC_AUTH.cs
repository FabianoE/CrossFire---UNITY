using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.CLIENT.Auth
{
    class REC_AUTH : Structure.PacketHandle
    {
        public override void Handler()
        {
            int packet = _packet.ReadInt();


            UnityEngine.Debug.LogWarning("PACKET::" + (Enums.PacketsAuth)packet);

            switch ((Enums.PacketsAuth)packet)
            {
                case Enums.PacketsAuth.Login:
                    REC_LOGIN.REC_LOGIN_DATA(_fromClient, _packet);
                    break;
            }
        }
    }
}
