using System;
using System.Collections.Generic;
using System.Text;

[System.Serializable]
public class Slot
{
    public int id;
    public Player player;

    public int exp;
    public int gold;

    public GameServer.Enums.SlotStatusEnum slotStatus;

    public Slot(int slot)
    {
        id = slot;
    }

    public void Clear()
    {
        player = null;
        exp = 0;
        gold = 0;
        //
        slotStatus = GameServer.Enums.SlotStatusEnum.EMPTY;
    }

    public void ChangeSlot(bool clear = false)
    {
        if (clear) 
        {
            Clear();
            return;
        }
    }
}
