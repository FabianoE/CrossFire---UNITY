using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REC_CHANGE_WEAPON : PacketHandle
{
    public override void Handler()
    {
        int _fromClient = _packet.ReadInt();
        int _weaponid = _packet.ReadInt();
        int lenght = _packet.ReadInt();
        byte[] bt = _packet.ReadBytes(lenght);
        WeaponInBattleInventory item = bt.DeserializeObject<WeaponInBattleInventory>();

        var wp = GameManager.players[_fromClient]._playerWeaponsManager.GetWeaponById(_weaponid);
        GameManager.players[_fromClient]._animController._animatorTPS.SetIntAnim("Gun", (int)wp.type);

        GameManager.players[_fromClient]._playerWeaponLocal.SetWeapon(_weaponid, item);
    }
}

