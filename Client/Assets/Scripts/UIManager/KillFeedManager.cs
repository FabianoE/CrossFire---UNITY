using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillFeedManager : MonoBehaviour
{
    [SerializeField] private Text Playername;
    [SerializeField] private Text Targetname;
    [SerializeField] private RawImage Weaponimage;
    [SerializeField] private RawImage KillType;
    [SerializeField] private RawImage NumKill;
    [SerializeField] private string Path;

    public void SetData(string player, string target, WeaponsInfo weapon, int type, int numkills)
    {
        Playername.text = player;
        Targetname.text = target;
        Weaponimage.texture = Resources.Load<Texture2D>("WeaponIcons/SHOT_WEAPON_" + weapon.Resource);

        string path2 = "";

        if (type == 10)
            KillType.gameObject.SetActive(true);
        else
            KillType.gameObject.SetActive(false);

        NumKill.gameObject.SetActive(true);

        if (numkills >= 7)
            path2 = "SHOT_MULTIKILLMAX";
        else if(numkills > 1)
            path2 = "SHOT_MULTIKILL"+numkills;
        else
            NumKill.gameObject.SetActive(false);

        NumKill.texture = Resources.Load<Texture2D>(Path+path2);

        Destroy(gameObject, 5f);
    }
}
