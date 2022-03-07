using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "Info_Weapons", menuName = "Scriptable Objects/Info Weapons", order = 1)]
public class InfoWeapons : ScriptableObject
{
    public List<WeaponsInfo> weaponsInfo;

    public WeaponsInfo GetWeaponById(int id)
    {
        return weaponsInfo.SingleOrDefault(x => x.ID == id);
    }
}

[Serializable]
public class WeaponsInfo
{
    public int ID;
    public string Name = "New Weapon";
    public string Resource = "New Resource";
    public string TPSAnimName = "";
    public float ReloadTime = 2f;
    public float posShotTime = 0;
    public float Weight = 3f;
    public float JumpForce = 2f;
    public WType type = WType.Rifle;

    public string CrossHair = null;
    public int Zoom = 0;
    [Space]
    [Header("UICrossHair Config")]
    public float IdleHair = 60f;
    public float WalkHair = 70f;
    public float FireHair = 80f;
    [Space(10)]
    public AudioClip[] ReloadSounds = new AudioClip[3];
    public AudioClip Select;
    public AudioClip[] Shoot = new AudioClip[3];
}


public enum WType
{
    Rifle,
    Sniper,
    Pistol,
    Knife,
    Grenade,
}
