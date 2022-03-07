using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.SERVER
{
    class SEND_SYNC
    {
        /// <summary>
        /// Send Sync Reload Weapon
        /// </summary>
        public static void SEND_DATA_SYNC(Player _player)
        {
            using(Packet packet = new Packet((int)ServerPackets.networkSync))
            {
                byte[] bt = _player.getWeaponEquiped().Serialize();
                packet.Write(1);
                packet.Write(_player.id);
                packet.Write(_player.getWeaponEquiped().ID);
                packet.Write(bt.Length);
                packet.Write(bt);
                packet.WriteLength();

                _player.room.SendData(packet);

                UnityEngine.Debug.LogWarning("RELOAD 2");
            }
        }
    }
}
