using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.SERVER
{
    class SEND_ROOM_DATA : Packet
    {
        public SEND_ROOM_DATA(Room room) //Sync Time
        {
            Write(598);
            Write(1);
            Write(room.GetTimeLeft());
            WriteLength();
        }

        public SEND_ROOM_DATA(Room room, Client player, Client target, int weaponid, int type) //SendKill
        {
            Write(598);
            Write(2);

            Write(room.type == Enums.RoomTypes.Kill ? room.RoomObjective : 0);

            if (room.mode == Enums.RoomMode.FFA)
            {
                Write(room.GetAcePlayer().KillInRoom);
                Write(room.GetAcePlayer().KillInRoom);
            }
            else
            {
                if (player.player.Team == (int)Enums.Teams.BlackList)
                {
                    Write(room.blkills);
                    Write(room.grkills);
                }
                else if (player.player.Team == (int)Enums.Teams.GlobalRisk)
                {
                    Write(room.grkills);
                    Write(room.blkills);
                }
            }
            UnityEngine.Debug.LogWarning($"KILLS {room.blkills} {room.grkills}");

            Write(player.id);
            Write(target.id);
            Write(weaponid);
            Write(type);
            Write(player.player.LastKills);
            WriteLength();
        }

        public SEND_ROOM_DATA(Client player) //New Player
        {
            Write(598);
            Write(3);

            Write(player.player.room.type == Enums.RoomTypes.Kill ? player.player.room.RoomObjective : 0);

            Write((int)player.player.room.mode); //Room mode

            Write(player.player.Team);
            if (player.player.Team == (int)Enums.Teams.BlackList)
            {
                Write(player.player.room.blkills);
                Write(player.player.room.grkills);
            }
            else
            {
                Write(player.player.room.grkills);
                Write(player.player.room.blkills);
            }
            WriteLength();
        }

        public SEND_ROOM_DATA(List<Player> lplayer, Room room) //Player ScoreBoard
        {
            byte[] bt = lplayer.ToArray().Serialize();
            //
            Write(598);
            Write(4);
            Write((int)room.mode);
            Write(bt.Length);
            Write(bt);
            WriteLength();
        }
    }
}
