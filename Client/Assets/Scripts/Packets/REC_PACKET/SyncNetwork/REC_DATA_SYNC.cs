using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REC_DATA_SYNC : PacketHandle
{
    public override void Handler()
    {
        int packet = _packet.ReadInt();

        switch (packet)
        {
            case 1: //Set Bullets Weapon Reloaded
                try
                {
                    int playerid = _packet.ReadInt();
                    int weaponid = _packet.ReadInt();
                    int packetlenght = _packet.ReadInt();
                    byte[] bt = _packet.ReadBytes(packetlenght);
                    WeaponInBattleInventory item = bt.DeserializeObject<WeaponInBattleInventory>();

                    GameManager.players[playerid]._playerWeaponLocal.GetWeapon().FpvModel.GetComponent<WeaponController>().SetWeaponData(item.AmmoinPaint, item.Ammo);
                }
                catch { }
                break;
        }
    }
}
