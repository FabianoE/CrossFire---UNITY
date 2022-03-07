using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEND_RESPAWN : MonoBehaviour
{
    public SEND_RESPAWN()
    {
        using (Packet packet = new Packet((int)ClientPackets.playerRespawn))
        {
            packet.WriteLength();
            Client.instance.tcp.SendData(packet);
        }
    }
}

public class REC_RESPAWN : PacketHandle
{
    public override void Handler()
    {
        int id = _packet.ReadInt();
        int health = _packet.ReadInt();

        PlayerManager player = GameManager.players[id].GetComponent<PlayerManager>();

        player.Alive();
        player.UISetHealth(health);
        
        foreach(WeaponLocal wp in player._playerWeaponLocal.GetAllWeapon())
        {
            wp.FpvModel.GetComponent<WeaponController>().WeaponRespawned();
        }
    }
}
