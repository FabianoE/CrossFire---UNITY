using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.SERVER.Lobby.Battle
{
    class SEND_STOP_BATTLE : Packet
    {
        public SEND_STOP_BATTLE(Room room)
        {
            List<Player> list = new List<Player>();

            foreach (var obj in room.slots)
                if (obj.slotStatus == Enums.SlotStatusEnum.BATTLE_READY)
                {
                    obj.player.Team = obj.id % 2;
                    list.Add(obj.player);
                    obj.slotStatus = Enums.SlotStatusEnum.NORMAL;
                }
            /////////////////////////////////////////////////////////
            byte[] bt = list.Serialize();

            Write((int)ServerPackets.lobbyData);
            Write((int)Enums.SubLobbyData.StopBattle);
            Write(room.blkills);
            Write(room.grkills);
            /////////// Set winner team
            if (room.mode != Enums.RoomMode.FFA)
            {
                if (room.blkills > room.grkills)
                    Write(0); //BL WIN
                else if (room.grkills > room.blkills)
                    Write(1); //GR WIN
                else
                    Write(2); //EMPATE
            }
            else
            {
                Write(0);
            }
            ///////////
            Write((int)room.mode);
            Write(bt.Length);
            Write(bt);

            WriteLength();
        }
    }
}
