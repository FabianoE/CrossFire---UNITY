using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEND_DAMAGE : MonoBehaviour
{
    public SEND_DAMAGE(int playerid, int targetid, int weaponid, int damage)
    {
        using (Packet packet = new Packet((int)ClientPackets.playerDamage))
        {
            packet.Write(playerid);
            packet.Write(targetid);
            packet.Write(weaponid);
            packet.Write(damage);
            packet.WriteLength();

            Client.instance.tcp.SendData(packet);
        }
    }
}

public class REC_DAMAGE : PacketHandle
{
    public override void Handler()
    {
        int sw = _packet.ReadInt();

        switch (sw)
        {
            case 1:
                try
                {
                    int player = _packet.ReadInt();
                    int target = _packet.ReadInt();
                    int weaponid = _packet.ReadInt();

                    var pManager = GameManager.players[player].GetComponent<PlayerManager>();
                    var tManager = GameManager.players[target].GetComponent<PlayerManager>();

                    pManager.Kills++;
                    tManager.Death++;

                    tManager.SetDeath();

                    if(Client.instance.myId == player)
                        UICrossHair.instance.OnHitMarker();//HitMarker
                }
                catch { }
                break;

            case 2:
                try
                {
                    int playerid = _packet.ReadInt(); //ID TO HITMARKER
                    int target = _packet.ReadInt();
                    int health = _packet.ReadInt();

                    var tManager = GameManager.players[target].GetComponent<PlayerManager>();

                    tManager.UISetHealth(health);
                    
                    if(Client.instance.myId == playerid)
                        UICrossHair.instance.OnHitMarker();//HitMarker
                }
                catch { }
                break;

            case 3:
                try
                {
                    Debug.LogWarning("KILL MARK");
                    int type = _packet.ReadInt();
                    int kill = _packet.ReadInt();
                    UIPVPManager.instance.KillMark(type ,kill);
                }
                catch { }
                break;

        }
    }
}
