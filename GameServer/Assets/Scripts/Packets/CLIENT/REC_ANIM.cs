using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace GameServer.Packets.CLIENT
{
    class REC_ANIM : Structure.PacketHandle
    {
        public override void Handler()
        {
            Packet pk = _packet;

            int playerid = _packet.ReadInt();
            int packet = _packet.ReadInt();

            switch (packet)
            {
                case 106: //Reload Packet
                    WeaponInBattleInventory item = _player.getWeaponEquiped();
                    if (item.Ammo > 0 && item.AmmoinPaint < item.MaxAmmo)
                    {
                        //new Thread(() =>
                        //{
                            _player.isReloading = true;
                            TimerAction.ActionReload(ServerObjectsManager.instance.ST_WeaponsData.GetWeaponById(item.ID).ReloadTime, _player);
                        //}).Start();
                    }
                    else
                        return;
                    break;
                    //////////////////
            }
            ServerSend.SendUDPDataToAll(pk);
        }
    }
}
