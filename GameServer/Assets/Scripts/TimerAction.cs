using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Cysharp.Threading.Tasks;

namespace GameServer
{
    class TimerAction
    {
        public static async UniTaskVoid RoomStartBattle(int delay, Room room)
        {
            await UniTask.Delay(delay);

            if(room != null)
            {
                room.roomStatus = Enums.RoomStatusEnum.BATTLE;
                room.StartBattle();
            }
        }

        public static async UniTaskVoid ActionReload(int delay, Player player)
        {
            player.ChangeWeaponCheck = 0;

            await UniTask.Delay(delay);

            if (player.ChangeWeaponCheck == 0 && player.Alive)
            {
                player.getWeaponEquiped().Reload();

                player.isReloading = false;

                Packets.SERVER.SEND_SYNC.SEND_DATA_SYNC(player);

                UnityEngine.Debug.LogWarning("[<->] Weapon Reloaded");
            }
            else
                return;
        }
    }
}
