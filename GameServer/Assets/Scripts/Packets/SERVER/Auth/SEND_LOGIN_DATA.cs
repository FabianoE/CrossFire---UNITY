using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.SERVER.Auth
{
    class SEND_LOGIN_DATA : Packet
    {
        public SEND_LOGIN_DATA(bool success)
        {
            Write((int)ServerPackets.authData);
            Write((int)Enums.PacketsAuth.Login);
            Write(success);

            WriteLength();
        }
    }
}
