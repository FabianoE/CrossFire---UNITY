using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pvp_Chat_Class : MonoBehaviour
{
    public Text Message;
    public Text MessageType_Text;
    public Image MessageType_BG;

    public void SetData(string playername, string message, int chat)
    {
        switch (chat)
        {
            case 1:
                MessageType_Text.text = "ALL";
                MessageType_BG.color = Color.white;
                break;
            case 2:
                MessageType_Text.text = "TEAM";
                MessageType_BG.color = Color.blue;
                break;
            case 3:
                MessageType_Text.text = "SYSTEM";
                MessageType_BG.color = Color.yellow;
                Message.text = "<color='YELLOW'>" + message + "</color>";
                return;
        }

        Message.text = playername + ":" + message;
    }
}
