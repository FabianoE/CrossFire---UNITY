using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.CLIENT.Lobby.Battle
{
    class REC_START_BATTLE
    {
        public static void Received(Packet packet, Player player)
        {
            UnityEngine.Debug.LogWarning($"O jogador {player.username} está aguardando...");

            player.room.slots[player.slotId].slotStatus = Enums.SlotStatusEnum.BATTLE_READY;

            player.room.roomStatus = Enums.RoomStatusEnum.BATTLE_READY;

            if (player.room.GetSlotLoaded().Count >= Constants.MinPlayer_RoomStart && player.room.roomStatus == Enums.RoomStatusEnum.BATTLE_READY)
            {
                TimerAction.RoomStartBattle(5000, player.room);
            }
            else if (player.room.roomStatus == Enums.RoomStatusEnum.BATTLE)
            {
                new Packets.SERVER.SEND_SPAWN(player.id);
            }
        }
    }
}
