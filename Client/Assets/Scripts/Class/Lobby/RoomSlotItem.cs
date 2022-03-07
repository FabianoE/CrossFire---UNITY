using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSlotItem : MonoBehaviour
{
    public int SLOTID;
    public int OwnerId;
    public bool isNull { get { return isNull; } set { isNullChanged(value); } }
    public bool Host = false;

    public Image playerPatent;
    public Image Background;
    public Image slotStatus;
    public Text player_Name;

    public void SetData(int playerid, string name, int status, bool ishost, int exp)
    {
        OwnerId = playerid;
        player_Name.text = name;
        Host = ishost;
        playerPatent.enabled = true;
        playerPatent.sprite = Resources.Load<Sprite>("UI/Class/BigClass_" + GetPatent.ReturnPatentByExp(exp));
        SlotChange(status);
    }

    public void SlotChange(int status)
    {
        if (Host)
        {
            slotStatus.enabled = true;
            slotStatus.sprite = Resources.Load<Sprite>("UI/RoomLobby/HostIcon");
            return;
        }

        switch (status)
        {
            case 4:
                slotStatus.enabled = false;
                break;
            case 5:
                slotStatus.enabled = true;
                slotStatus.sprite = Resources.Load<Sprite>("UI/RoomLobby/ReadyIcon");
                break;
            case 7:
                slotStatus.enabled = true;
                slotStatus.sprite = Resources.Load<Sprite>("UI/RoomLobby/ReadyIcon");
                break;
        }
    }

    public void onClick()
    {
        if (OwnerId == 0)
            SEND_CHANGE_SLOT.onSendChangeSlot(SLOTID);
    }

    void isNullChanged(bool value)
    {
        if (value)
        {
            OwnerId = 0;
            playerPatent.enabled = false;
            player_Name.text = null;
            slotStatus.sprite = null;
            slotStatus.enabled = false;
            Host = false;
        }
    }


}
