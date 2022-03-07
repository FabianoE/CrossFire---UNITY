using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class REC_LOBBY_DATA : PacketHandle
{
    public override void Handler()
    {
        int subLobbyPacket = _packet.ReadInt();

        switch (subLobbyPacket)
        {
            case (int)LobbyEnums.SubLobbyData.Inventory:
                REC_INVENTORY_DATA.Received(_packet);
                break;

            case (int)LobbyEnums.SubLobbyData.PlayerInfo:
                REC_PLAYER_INFO.Received(_packet);
                break;

            #region Enter, Leave, Slot Changed and List of ROOM
            case (int)LobbyEnums.SubLobbyData.EnterRoom:
                REC_ENTER_ROOM.Received(_packet);
                break;
            case (int)LobbyEnums.SubLobbyData.LeaveRoom:
                REC_LEAVE_ROOM.Received(_packet);
                break;
            case (int)LobbyEnums.SubLobbyData.RoomList:
                REC_ROOM_LIST.Received(_packet);
                break;
            case (int)LobbyEnums.SubLobbyData.SlotChange:
                REC_SLOT_CHANGE.Received(_packet);
                break;
            #endregion

            case (int)LobbyEnums.SubLobbyData.Chat:
                REC_CHAT.Received(_packet);
                break;

            #region Battle
            case (int)LobbyEnums.SubLobbyData.StartBattle:
                REC_BATTLE_START.Received(_packet);
                break;
            case (int)LobbyEnums.SubLobbyData.StopBattle:
                REC_BATTLE_STOP.Received(_packet);
                break;
                #endregion
        }
    }
}
