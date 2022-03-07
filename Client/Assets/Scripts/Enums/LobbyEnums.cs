using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LobbyEnums
{
    public enum LobbyBoxEnum
    {
        Lobby = 1,
        Inventory,
        Room,
        CreateRoom,
    }

    public enum SubLobbyData
    {
        Inventory = 1,
        InventoryEquip,
        PlayerInfo,
        EnterRoom,
        LeaveRoom,
        CreateRoom,
        RoomList,
        ReadyRoom,
        SlotChange,
        Chat,
        //
        StartBattle,
        StopBattle,
    }

    public enum PacketAuth
    {
        Login = 1,
    }
}
