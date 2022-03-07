using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class UIPVPManager : MonoBehaviour
{

    public static UIPVPManager instance;

    [Space(5)]
    [Header("Hud In PVP")]
    [Space(2)]
    [Header("HUD WEAPON")]
    public Text AmmoInPaint;
    public Text AllAmmo;
    public Text WeaponName;
    public RawImage WeaponIcon;
    public RawImage WeaponIconLine;
    [Space(5)]
    [Header("HUD PLAYER")]
    public Text AcText;
    public Text HpText;
    [Space(2)]
    [Header("HUD SCOREBOARD")]
    public Image MyTeamImage;
    public Image OtherTeamImage;
    //
    public Text MyTeamKills;
    public Text OtherTeamKills;
    public Text Kill_Round;
    public Text TimeRoom;
    [Space(5)]
    [Header("HUD KillFeed")]
    public Transform KillFeedSpawnLocal;
    public GameObject KillFeedObject;
    [Space(5)]
    [Header("HUD KILL Mark")]
    public Image KillMarkImage;
    public string KillMarkPath;
    [Space(5)]
    [Header("MiniMap")]
    public RawImage MiniMapImage;
    [Space(5)]
    [Header("CrossHair")]
    public Image CrossHairImage;
    public RectTransform DynamicCrossHair;
    public float speed;
    [Header("SPAWN PLAYER")]
    public GameObject spawnLocation;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance Error");
            Destroy(this);
        }
    }

    private void Start()
    {
        MiniMapImage.texture = Resources.Load<RenderTexture>("MiniMap/Render/Map");
        gameObject.SetActive(false);
    }

    public void SetWeapon(WeaponsInfo weapon)
    {
        WeaponIcon.texture = Resources.Load<Texture2D>("UI/AMMOICON/ICON/" + weapon.Resource + "_BG");
        WeaponIconLine.texture = Resources.Load<Texture2D>("UI/AMMOICON/ICON/" + weapon.Resource + "_LINE");
        WeaponName.text = weapon.Name;
    }

    public void SetWeaponUIData(WeaponController data)
    {
        int bl = data.bullets;
        int am = data.ammosb;

        AmmoInPaint.text = bl.ToString();
        AllAmmo.text = am.ToString();
    }

    public void SetHP_AC(int ac, int hp)
    {
        AcText.text = ac.ToString();
        HpText.text = hp.ToString();
    }

    public void SetScoreBoard(int myteam, int otherteam, int kill_round)
    {
        MyTeamKills.text = myteam.ToString();
        OtherTeamKills.text = otherteam.ToString();
        if (kill_round != 0)
            Kill_Round.text = kill_round.ToString();
        else
            Kill_Round.text = "";
    }

    public void SetScoreBoard(string timeRoom)
    {
        TimeRoom.text = timeRoom;
    }

    public void SetImageScoreBoard(int team)
    {
        if (team == 0)
        {
            MyTeamImage.sprite = Resources.Load<Sprite>("UI/HUD/Sprite/RoundBoard_BL");
            OtherTeamImage.sprite = Resources.Load<Sprite>("UI/HUD/Sprite/RoundBoard_GR");
        }
        else if (team == 1)
        {
            MyTeamImage.sprite = Resources.Load<Sprite>("UI/HUD/Sprite/RoundBoard_GR");
            OtherTeamImage.sprite = Resources.Load<Sprite>("UI/HUD/Sprite/RoundBoard_BL");
        }
        else
        {
            MyTeamImage.sprite = Resources.Load<Sprite>("UI/HUD/Sprite/RoundBoard_BL");
            OtherTeamImage.sprite = Resources.Load<Sprite>("UI/HUD/Sprite/RoundBoard_DM");
        }
    }

    public void SetCrossHair(string cname, bool enabled)
    {
        CrossHairImage.sprite = Resources.Load<Sprite>("UI/CrossHair/" + cname);
        CrossHairImage.gameObject.SetActive(enabled);
    }

    public void SpawnKillFeed(PlayerManager player, PlayerManager target, int weaponid, int type, int num)
    {
        GameObject inst = Instantiate(KillFeedObject, KillFeedSpawnLocal);
        var weapon = player._playerWeaponsManager.GetWeaponById(weaponid);
        inst.GetComponent<KillFeedManager>().SetData(player.username, target.username, weapon, type, num);
    }

    public void KillMark(int type, int kill)
    {
        string path = "";
        KillMarkImage.gameObject.SetActive(false);

        if (type == 10 && kill == 1)
            path = "badge_headshot_gold";
        else if (kill >= 6)
            path = "badge_multi6";
        else
            path = "badge_multi" + kill;

        KillMarkImage.sprite = Resources.Load<Sprite>(KillMarkPath + path);

        KillMarkImage.gameObject.SetActive(true);

    }
}
