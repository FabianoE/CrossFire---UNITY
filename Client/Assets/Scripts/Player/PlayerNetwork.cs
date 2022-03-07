using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : IsMine
{
    #region SyncLatency

    public float speedsend = 5f;
    private float lastLatency = 0f;

    #endregion

    private void Update()
    {
        if(Time.time >= lastLatency)
        {
            SendLatency();
            lastLatency = Time.time + 6f;
        }
    }

    #region SendPackets
    public void SendChangeWeapon(int id)
    {
        //new SEND_CHANGE_WEAPON(id);
        SEND_CHANGE_WEAPON.CHANGE_WEAPON(id);
    }

    public void SendDamage(int playerid, int targetid, int weaponid, int damage)
    {  
         Debug.LogError("SEND DAMAGE2");
         new SEND_DAMAGE(playerid, targetid, weaponid, damage);
    }

    public void SendLatency()
    {
        Client.instance.tcp.RetrievePing();
        SEND_NETWORKDATA.SendLatency();
    }


    #endregion

}
