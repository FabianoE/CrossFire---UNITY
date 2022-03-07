using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public string Code, Name;
    public int ID;
    public int Damage, Perna, Peito, Cabeca, Pe, Braco;
    public int Ammo, Maxammo, Ammoinpaint;
}

[Serializable]
public class WeaponInBattleInventory
{
    public int ID;
    public int Ammo;
    public int AmmoinPaint;
    public int MaxAmmo;
}
