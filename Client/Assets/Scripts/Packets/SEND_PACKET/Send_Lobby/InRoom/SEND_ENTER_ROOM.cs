using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    class SEND_ENTER_ROOM
    {
        public SEND_ENTER_ROOM(int roomid)
        {
            using(Packet packet = new Packet((int)ClientPackets.lobbyData))
            {
                packet.Write((int)LobbyEnums.SubLobbyData.EnterRoom);
                packet.Write(roomid);
                packet.WriteLength();

                Client.instance.tcp.SendData(packet);
            }
        }
    }
