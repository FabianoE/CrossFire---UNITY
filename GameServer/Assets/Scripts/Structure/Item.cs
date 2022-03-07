using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace GameServer
//{

/*
 Perna = 2
 Pé = 3
 Peito = 4
 Braço = 5
 Cabeça = 10
 */
/*[Serializable]
public class Item
{
    public string Code, Name;
    public int ID;
    public int Damage, Perna, Peito, Cabeca, Pe, Braco;
    public int Ammo, Maxammo, Ammoinpaint;
    public int Reloadtime;
    [NonSerialized] public string WeaponType;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="code"></param>
    /// <param name="name"></param>
    /// <param name="weapontype"></param>
    /// <param name="damage"></param>
    /// <param name="perna"></param>
    /// <param name="peito"></param>
    /// <param name="cabeca"></param>
    /// <param name="pe"></param>
    /// <param name="braco"></param>
    /// <param name="ammo"></param>
    /// <param name="maxammo"></param>
    /// <param name="ammoinpaint"></param>
    /// <param name="reloadtime"></param>
    public Item(int id, string code, string name, string weapontype, int damage, int perna, int peito, int cabeca, int pe, int braco, int ammo, int maxammo, int ammoinpaint, int reloadtime)
    {
        this.ID = id;
        this.Code = code;
        this.Name = name;
        this.Damage = damage;
        this.Perna = perna;
        this.Peito = peito;
        this.Cabeca = cabeca;
        this.Pe = pe;
        this.Braco = braco;
        this.Ammo = ammo;
        this.Ammoinpaint = ammoinpaint;
        this.Maxammo = maxammo;
        this.Reloadtime = reloadtime;
        this.WeaponType = weapontype;
    }

    public void Reload()
    {
        try
        {
            UnityEngine.Debug.LogWarning("[<->] WEAPON RELOADED");
            for (int i = 0; i < Maxammo; i++)
            {
                if (Ammo > 0 && Ammoinpaint < Maxammo)
                {
                    Ammoinpaint++;
                    Ammo--;
                }
                else
                {
                    break;
                }
            }
        }
        catch
        {
            UnityEngine.Debug.LogWarning("[<->] ERROR IN RELOAD");
        }
    }


    public int TDamage(int part)
    {
        switch (part)
        {
            case 2:
                return Perna;
            case 3:
                return Pe;
            case 4:
                return Peito;
            case 5:
                return Braco;
            case 10:
                return Cabeca;
        }
        return 0;
    }
}

public class ItemManager
{
    public static Dictionary<string, Item> AllItems;

    public static void LoadItems()
    {
        AllItems = new Dictionary<string, Item>()
            {
                //ID,         ID, CODE, NAME, WEAPONTYPE, DAMAGE, PERNA, PEITO, CABECA, PE, BRAÇO, <= Damage == Bullet => AMMO, MAX AMMO, AMMO IN PAINT, RELOAD TIME
                {"1", new Item(1, "1", "AK47", "Rifle", 10, 24, 40, 100, 24, 35, /* <= Damage == Bullet => */// 90, 30, 30, /**/ 2000)},
               // {"2", new Item(2, "2", "DESERT", "Pistol", 7, 35, 61, 100, 35, 51, /* <= Damage == Bullet => */  21, 7, 7,  /**/ 2400)},
               // {"3", new Item(3, "3", "AWM INFERNAL", "Sniper", 100, 81, 100, 100, 81, 100, /* <= Damage == Bullet => */ 20, 5, 5, /**/ 2000)},
               // {"4", new Item(4, "4", "MACHADO", "Knife", 100, 90, 90, 100, 60, 60, /*<= Damage == Bullet =>*/ 0,0,0,0)},
               // {"5", new Item(5, "5", "GRENADE", "Grenade", 100, 100, 100, 100, 100, 100, /*<= Damage == Bullet =>*/ 0,0,0,0)},
           // };
    //}

   // public static Item GetItemById(int id)
   // {
     //   return AllItems.Values.Where(x => x.ID == id).FirstOrDefault();
   // }
//}

public class RoomInventory
{
    public List<WeaponInBattleInventory> BagsAllWeapons;

    public WeaponInBattleInventory Primary;
    public WeaponInBattleInventory Secondary;
    public WeaponInBattleInventory Melee;
    public WeaponInBattleInventory Grenade;

    public void EquipWeapon(WeaponInBattleInventory data)
    {
        var Weapon = ServerObjectsManager.instance.ST_WeaponsData.GetWeaponById(data.ID);
        switch (Weapon.WeaponType)
        {
            case WType.Rifle:
            case WType.Sniper:
                Primary = new WeaponInBattleInventory(data);
                break;
            case WType.Pistol:
                Secondary = new WeaponInBattleInventory(data);
                break;
            case WType.Knife:
                Melee = new WeaponInBattleInventory(data);
                break;
            case WType.Grenade:
                Grenade = new WeaponInBattleInventory(data);
                break;
            default:
                return;
        }
    }

    public void EquipWeapon(WeaponsData_Class data)
    {
        switch (data.WeaponType)
        {
            case WType.Rifle:
            case WType.Sniper:
                Primary = new WeaponInBattleInventory(data);
                break;
            case WType.Pistol:
                Secondary = new WeaponInBattleInventory(data);
                break;
            case WType.Knife:
                Melee = new WeaponInBattleInventory(data);
                break;
            case WType.Grenade:
                Grenade = new WeaponInBattleInventory(data);
                break;
            default:
                return;
        }
    }

    public WeaponInBattleInventory GetEquipedWeapon(int ID)
    {
        var Weapon = ServerObjectsManager.instance.ST_WeaponsData.GetWeaponById(ID);

        switch (Weapon.WeaponType)
        {
            case WType.Rifle:
            case WType.Sniper:
                return Primary;
            case WType.Pistol:
                return Secondary;
            case WType.Knife:
                return Melee;
            case WType.Grenade:
                return Grenade;
        }

        return null;
    }
}

[Serializable]
public class WeaponInBattleInventory
{
    public int ID;
    public int Ammo; //Quantidade de tiros restantes nos outros pentes
    public int AmmoinPaint; //Quantidade de tiro restante no pente atual
    public int MaxAmmo; //Quantidade maxima para o pente atual

    public WeaponInBattleInventory(WeaponInBattleInventory data)
    {
        ID = data.ID;
        Ammo = data.Ammo;
        AmmoinPaint = data.AmmoinPaint;
        MaxAmmo = data.MaxAmmo;
    }

    public WeaponInBattleInventory(WeaponsData_Class data)
    {
        ID = data.ID;
        Ammo = data.Ammo;
        AmmoinPaint = data.AmmoinPaint;
        MaxAmmo = data.MaxAmmo;
    }

    public void Reload()
    {
        for (int i = 0; i < MaxAmmo; i++)
        {
            if (Ammo > 0 && AmmoinPaint < MaxAmmo)
            {
                AmmoinPaint++;
                Ammo--;
            }
            else
            {
                break;
            }
        }
    }
}
