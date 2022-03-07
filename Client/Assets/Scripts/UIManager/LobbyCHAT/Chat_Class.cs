using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat_Class : MonoBehaviour
{
    public Text Message;

    public void SetText(string playername, string msg)
    {
        string color = playername == "SYSTEM" ? "YELLOW" : "White";
        playername = playername == null ? null : playername + " : ";
        Message.text = "<color='"+ color +"'>" + playername + "</color><color='#8C8C8C'>"+ msg +"</color>";
    }
}
