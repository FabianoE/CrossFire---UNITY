using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.SERVER
{
    class SEND_CHANGE_WEAPON
    {
        public SEND_CHANGE_WEAPON(Player _player, int weapon)
        {
            if (weapon == 1 && _player.weapon.ToString() == _player.primaryweaponid || weapon == 2 && _player.weapon.ToString() == _player.secondaryweapon || weapon == 3 && _player.weapon.ToString() == _player.tercweapon || weapon == null)
                return;

            //if (weapon == 4) //Grenade is Disabled
            //    return;

            if (weapon == 1)
                _player.weapon = int.Parse(_player.primaryweaponid);
            else if (weapon == 2)
                _player.weapon = int.Parse(_player.secondaryweapon);
            else if (weapon == 3)
                _player.weapon = int.Parse(_player.tercweapon);
            else if (weapon == 4)
                _player.weapon = int.Parse(_player.grenadeweapon);

            weapon = _player.weapon;
            //UnityEngine.Debug.LogWarning("CHANGE WEAPON BULLETS " + _player.getWeaponEquiped().AmmoinPaint);

            _player.ChangeWeaponCheck++;
            _player.isReloading = false;

            using (Packet packet = new Packet((int)ServerPackets.playerChangeWeapon))
            {
                byte[] bt = _player.getWeaponEquiped().Serialize();
                packet.Write(_player.id);
                packet.Write(weapon);
                packet.Write(bt.Length);
                packet.Write(bt);

                packet.WriteLength();
                _player.room.SendData(packet);
            }
        }
    }
}
