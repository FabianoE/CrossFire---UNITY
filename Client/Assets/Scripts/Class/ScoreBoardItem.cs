using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardItem : MonoBehaviour
{
    public Image Player_Icon;
    public Text Player_Name;
    public Text Player_Clan;
    public Text Player_KD;
    public Text Player_Ping;

    private void Awake()
    {
        ClearData();
    }

    public void ClearData()
    {
        Player_Icon = null;
        Player_Name.text = "";
        Player_Clan.text = "Clã GM";
        Player_KD.text = "";
        Player_Ping.text = "";

        gameObject.SetActive(false);
    }

    public void SetData(Player player, bool otherteam = false)
    {
        gameObject.SetActive(true);

        Player_Icon = null;
        Player_Name.text = player.username;
        Player_Clan.text = "Clã GM";
        Player_KD.text = player.KillInRoom + "/" + player.DeathInRoom;
        Player_Ping.text = player.ping;


        if (otherteam == true)
            if (player.id == Client.instance.myId)
                GetComponent<Image>().color = new Color32(80, 80, 80, 199);
            else
                GetComponent<Image>().color = new Color32(48, 48, 48, 199);
        else
        {
            if (player.Team == 0) //BL
            {
                GetComponent<Image>().color = new Color32(221, 122, 47, 199);
            }
            else //GR
            {
                GetComponent<Image>().color = new Color32(0, 111, 255, 199);
            }
        }
    }
}
