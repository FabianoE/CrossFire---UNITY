using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreBoard : MonoBehaviour
{
    [Header("CE")]
    public GameObject CE;
    public GameObject SpawnItem_CE;
    public Transform SpawnPosition_CE_MyTeam;
    public Transform SpawnPosition_CE_OtherTeam;
    [Space]
    [Header("FFA")]
    public GameObject FFA;
    [Space]
    [Header("CE CONFIG SCORE")]
    public Text myTeam;
    public Image myTeamImg1;
    public Image myTeamImg2;
    [Space]
    public Text otherTeam;
    public Image otherImg1;
    public Image otherImg2;
    [Space]
    [Header("ScoreBoard List")]
    public ScoreBoardItem[] Team1;
    public ScoreBoardItem[] Team2;
    [Space]
    [Header("ScoreBoard List -- FFA")]
    public ScoreBoardItem[] TeamALL;

    public static UIScoreBoard instance;
    //
    /*
     Colors
        GR: 
        {
            Background: #006FFF
            Effect: #008FFF
            PlayerScore Background: #006FFF
        }
        BL:
        {
            Background: #FF6D00
            Effect: #FF8000
            PlayerScore Background: #DD7A2F
        }
        Other Team:
        {
            Background: #3F3F3F
            Effect: #848484
            PlayerScore Background: #303030
        }
     */

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        //gameObject.SetActive(false);
    }

    public void DefineUI(int team)
    {
        if (team == 0)//BL
        {
            myTeam.text = "Black List";
            otherTeam.text = "Global Risk";
            myTeamImg1.color = new Color32(255, 109, 0, 137);
            myTeamImg2.color = new Color32(132, 132, 132, 51);
        }
        else //GR
        {
            myTeam.text = "Global Risk";
            otherTeam.text = "Black List";
            myTeamImg1.color = new Color32(0, 111, 255, 137);
            myTeamImg2.color = new Color32(0, 143, 255, 51);
        }
    }

    public void SetUI(List<Player> lsPlayer, int roommode)
    {
        List<Player> mTeam = new List<Player>();
        List<Player> oTeam = new List<Player>();

        int t1 = 0;
        int t2 = 0;

        if (roommode != 1)
        {
            CE.SetActive(true);
            FFA.SetActive(false);
        }
        else
        {
            CE.SetActive(false);
            FFA.SetActive(true);
        }

        if (roommode != 1) //Dont is FFA
        {
            lsPlayer.ForEach(x =>
            {
                if (x.Team == Client.instance.myTeam)
                    mTeam.Add(x);
                else
                    oTeam.Add(x);
            });

            for (int i = 0; i < 8; i++)
            {
                Team1[i].ClearData();
                Team2[i].ClearData();
            }

            foreach (var obj in mTeam.OrderByDescending(x => x.KillInRoom))
            {
                Team1[t1].SetData(obj);
                t1++;
            }

            foreach (var obj in oTeam.OrderByDescending(x => x.KillInRoom))
            {
                Team2[t2].SetData(obj);
                t2++;
            }
        }
        else
        {
            foreach(var obj in lsPlayer.OrderByDescending(x => x.KillInRoom).OrderBy(x => x.DeathInRoom))
            {
                TeamALL[t1].SetData(obj, true);
                t1++;
            }
        }
    }

    /*public void SetUI(List<Player> lsPlayer)
    {
        List<Player> mTeam = new List<Player>();
        List<Player> oTeam = new List<Player>();

        lsPlayer.ForEach(x =>
        {
            if (x.Team == Client.instance.myTeam)
                mTeam.Add(x);
            else
                oTeam.Add(x);
        });

        //Delete MyTeam
        for(int i = 0; i < SpawnPosition_CE_MyTeam.childCount; i++)
        {
            Destroy(SpawnPosition_CE_MyTeam.GetChild(i).gameObject);
        }

        //Delete OtherTeam
        for (int i = 0; i < SpawnPosition_CE_OtherTeam.childCount; i++)
        {
            Destroy(SpawnPosition_CE_OtherTeam.GetChild(i).gameObject);
        }

        //Spawn MyTeam
        foreach (Player p in mTeam.OrderBy(x => x.KillInRoom))
        {
            GameObject ins = Instantiate(SpawnItem_CE, SpawnPosition_CE_MyTeam);
            ins.GetComponent<ScoreBoardItem>().SetData(p);
        }

        //Spawn OtherTeam
        foreach (Player p in oTeam.OrderBy(x => x.KillInRoom))
        {
            GameObject ins = Instantiate(SpawnItem_CE, SpawnPosition_CE_OtherTeam);
            ins.GetComponent<ScoreBoardItem>().SetData(p, true);
        }
    }*/
}
