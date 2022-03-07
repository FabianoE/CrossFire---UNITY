using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerWeaponsObjects : MonoBehaviour
{
    public PlayerManager _player;
    public WeaponLocal[] _weaponLocal;

    public WeaponLocal GetWeaponById(int id)
    {
        return _weaponLocal.SingleOrDefault(x => x.ID == id);
    }

    public WeaponLocal GetWeapon()
    {
        return _weaponLocal.SingleOrDefault(x => x.ID == _player.WeaponID);
    }

    public WeaponLocal[] GetAllWeapon()
    {
        return _weaponLocal;
    }

    public void SetIntWeaponAnim(string name, int status)
    {
        GetWeaponById(_player.WeaponID).FpvModel.GetComponent<Animator>().SetInteger(name, status);
    }
    public void StartWeaponAnim(string name)
    {
        GetWeaponById(_player.WeaponID).FpvModel.GetComponent<Animator>().Play(name);
    }

    public void StartWeaponAnim(string name, int i, int j)
    {
        GetWeaponById(_player.WeaponID).FpvModel.GetComponent<Animator>().Play(name, i, j);
    }

    public void SetWeapon(int id, WeaponInBattleInventory item = null)
    {
        var weapon = GetWeaponById(id);
        _player.WeaponID = id;

        //Script to desactive others weapons
        foreach(var w in _weaponLocal.Where(x => x.ID != id))
        {
            w.FpvModel.SetActive(false);
            w.TpvModel.SetActive(false);
        }

        weapon.FpvModel.SetActive(true);
        weapon.TpvModel.SetActive(true);

        if(item != null)
          weapon.FpvModel.GetComponent<WeaponController>().SetWeaponData(item.AmmoinPaint, item.Ammo);

        var tp = _player._playerWeaponsManager.GetWeaponById(id);
        int t = tp.type == WType.Rifle ? 0 : 1;

        _player._soundManager.SelectWeapon(tp);
        _player._playerController.WeaponWeight = tp.Weight;


        if (_player.isMine())
        {
            UIPVPManager.instance.SetWeapon(tp);
        }

        switch (tp.type)
        {
            case WType.Rifle:
            case WType.Sniper:
                _player._animController._animatorTPS.StartAnim("M4_Idle");
                break;
            case WType.Pistol:
                _player._animController._animatorTPS.StartAnim("Pistol_Idle");
                break;
            case WType.Knife:
                _player._animController._animatorTPS.StartAnim("Knife_Idle");
                break;
            case WType.Grenade:
                _player._animController._animatorTPS.StartAnim("Grenade_Idle");
                break;
        }

    }

}

[Serializable]
public class WeaponLocal
{
    public int ID;
    public GameObject FpvModel;
    public GameObject TpvModel;
}


