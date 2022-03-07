using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

class REC_SPAWN : PacketHandle
{
    public override void Handler()
    {
        LobbyItemsObject.instance.gameObject.SetActive(false);

        int _id = _packet.ReadInt();
        int myteam = _packet.ReadInt();
        int weaponid = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        Debug.LogError("ID USER: " + _id);

        int pkl = _packet.ReadInt();
        byte[] bt = _packet.ReadBytes(pkl);

        WeaponInBattleInventory it = bt.DeserializeObject<WeaponInBattleInventory>();

        GameManager.instance.SpawnpP(_id, myteam, _username, RespawnManager.instance.GetRespawn(myteam), _rotation, (() => {
            GameManager.players[_id]._playerWeaponLocal.SetWeapon(weaponid, it);
        }));
    }

}
