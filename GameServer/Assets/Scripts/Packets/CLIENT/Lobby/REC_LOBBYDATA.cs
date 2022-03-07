using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.CLIENT.Lobby
{
    class REC_LOBBYDATA : Structure.PacketHandle
    {
        public override void Handler()
        {
            int packet = _packet.ReadInt();

            switch (packet)
            {
                case (int)Enums.SubLobbyData.InventoryEquip:
                    REC_INVENTORY_EQUIP.Received(_packet, _player);
                    break;
                //////////////////////////////////////////////
                case (int)Enums.SubLobbyData.EnterRoom:
                    REC_ENTER_ROOM.Received(_packet, _player);
                    break;
                case (int)Enums.SubLobbyData.LeaveRoom:
                    REC_LEAVE_ROOM.Received(_packet, _player);
                    break;
                case (int)Enums.SubLobbyData.CreateRoom:
                    REC_CREATE_ROOM.Received(_packet, _player);
                    break;
                case (int)Enums.SubLobbyData.ReadyRoom:
                    REC_READY_ROOM.Received(_packet, _player);
                    break;
                case (int)Enums.SubLobbyData.SlotChange:
                    REC_SLOT_CHANGE.Received(_packet, _player);
                    break;
                //////////////////////////////////
                case (int)Enums.SubLobbyData.Chat:
                    REC_CHAT.Received(_packet, _player);
                    break;
                /////////////////////////////////
                case (int)Enums.SubLobbyData.StartBattle:
                    Battle.REC_START_BATTLE.Received(_packet, _player);
                    break;
            }
        }
    }
}
