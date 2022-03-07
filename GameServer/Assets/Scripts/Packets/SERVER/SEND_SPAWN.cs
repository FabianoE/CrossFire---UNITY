using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer.Packets.SERVER
{
    class SEND_SPAWN
    {
        public SEND_SPAWN(int _fromClient)
        {
            Player pl = Server.clients[_fromClient].player;

            pl.weapon = int.Parse(pl.primaryweaponid);

            SendSpawnPlayer(_fromClient, pl);
        }

        public void SendSpawnPlayer(int _toClient, Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
            {
                WeaponInBattleInventory it = _player.roomInventory.GetEquipedWeapon(_player.weapon);
                byte[] bt = it.Serialize();
                _packet.Write(_player.id);

                //////////////IF IS FFA
                if (_player.room.mode != Enums.RoomMode.FFA)
                    _packet.Write(_player.Team);
                else
                    _packet.Write(3);
                /////////////
                _packet.Write(_player.weapon);
                _packet.Write(_player.username);
                _packet.Write(_player.position);
                _packet.Write(_player.rotation);
                _packet.Write(bt.Length);
                _packet.Write(bt);

                _packet.WriteLength();

                _player.room.SendToPlayerInBattle(_packet);
                //ServerSend.SendTCPData(_toClient, _packet);
            }
        }
    }
}
