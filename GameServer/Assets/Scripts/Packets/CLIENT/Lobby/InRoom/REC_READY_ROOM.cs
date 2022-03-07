using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.CLIENT.Lobby
{
    class REC_READY_ROOM
    {
        public static void Received(Packet packet, Player player)
        {
            if (player.id == player.room.hostid && !player.room.started)
            {
               player.room.started = true;

               foreach (Client client in player.room.users.Values)
               {
                    if (client.player.room.GetSlotPlayer(client.player).slotStatus != Enums.SlotStatusEnum.READY && player.room.hostid != client.player.id)
                        continue;

                    UnityEngine.Debug.LogWarning("SEND SPAWN: " + client.id);

                    client.player.room.StartUDP(client);

                    client.player.room.GetSlotPlayer(client.player).slotStatus = Enums.SlotStatusEnum.BATTLE_READY;

                    client.tcp.SendData(new Packets.SERVER.Lobby.Battle.SEND_START_BATTLE());

                    //new Packets.SERVER.SEND_SPAWN(client.player.id, client.player.username);
               }
            }
            else if (!player.room.started)
            {
                Slot slot = player.room.GetSlotPlayer(player);
                slot.slotStatus = slot.slotStatus == Enums.SlotStatusEnum.READY ? Enums.SlotStatusEnum.NORMAL : Enums.SlotStatusEnum.READY;

                player.room.SendData(new SERVER.Lobby.InRoom.SEND_SLOT_CHANGE(slot));
            }
            else if (player.room.started)
            {
                Client client = Server.clients[player.id];
                client.player.room.GetSlotPlayer(player).slotStatus = Enums.SlotStatusEnum.BATTLE_READY;
                client.tcp.SendData(new Packets.SERVER.Lobby.Battle.SEND_START_BATTLE());
                /*foreach (Client client in player.room.users.Values)
                {
                    UnityEngine.Debug.LogWarning("SEND SPAWN: " + client.id);
                    client.player.room.StartUDP(client);

                    new Packets.SERVER.SEND_SPAWN(client.player.id);
                }*/
            }
        }
    }
}
