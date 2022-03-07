using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayerCollider : MonoBehaviour
{
    public int Part;
    public PlayerManager playerManager;

    public void Attack(Client client, int playerid, int targetid, int weaponid, int damage)
    {
        //if (client.myId == Client.instance.myId)
        if (playerid == Client.instance.myId)
            playerManager._playerNetwork.SendDamage(playerid, targetid, weaponid, damage);
    }
}
