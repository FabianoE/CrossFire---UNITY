using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Enums
{
    public enum Teams
    {
        None = -1,
        BlackList,
        GlobalRisk,
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

    public enum PacketsAuth
    {
        Login = 1,
    }

    public enum SlotStatusEnum
    {
        EMPTY,
        CLOSE,
        SHOP,
        INVENTORY,
        NORMAL,
        READY,
        BATTLE_READY,
        BATTLE,
    }

    public enum RoomStatusEnum 
    {
        NORMAL,
        READY,
        BATTLE_READY,
        BATTLE,
    }

    public enum RoomMode
    {
        TDM,
        FFA,
    }

    public enum RoomTypes
    {
        Kill,
        Time,
    }
}
