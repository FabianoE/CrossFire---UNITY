using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class REC_AUTH_DATA : PacketHandle
{
    public override void Handler()
    {
        int packet = _packet.ReadInt();

        switch ((LobbyEnums.PacketAuth)packet)
        {
            case LobbyEnums.PacketAuth.Login:
                Auth_Login.LoginReceived(_packet);
                break;
        }
    }
}
