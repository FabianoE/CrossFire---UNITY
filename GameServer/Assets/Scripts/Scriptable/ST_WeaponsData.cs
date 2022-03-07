using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ST_WeaponsData", menuName = "ScriptTableObjects/ST_WeaponsData", order = 1)]
public class ST_WeaponsData : ScriptableObject
{
    public List<WeaponsData_Class> _WeaponsData;

    public WeaponsData_Class GetWeaponById(int ID)
    {
        return _WeaponsData.Find(x => x.ID == ID);
    }

    public int GetDamageByPart(int id, int part)
    {
        WeaponsData_Class data = GetWeaponById(id);

        switch (part)
        {
            case 2:
                return data.Perna;
            case 3:
                return data.Pe;
            case 4:
                return data.Peito;
            case 5:
                return data.Braco;
            case 10:
                return data.Cabeca;
        }
        return 0;
    }
}

[Serializable]
public class WeaponsData_Class
{
    public int ID;
    public string Code;
    public string Name;
    //[NonSerialized] 
    public WType WeaponType;
    [Space]
    public int Damage;
    public int Perna;
    public int Peito;
    public int Cabeca;
    public int Pe;
    public int Braco;
    [Space]
    public int Ammo;
    public int MaxAmmo;
    public int AmmoinPaint;
    [Space]
    public int ReloadTime;
    public int FireRate;
    public int DrawDelay = 1000;

}

public enum WType
{
    Rifle,
    Sniper,
    Pistol,
    Knife,
    Grenade,
}