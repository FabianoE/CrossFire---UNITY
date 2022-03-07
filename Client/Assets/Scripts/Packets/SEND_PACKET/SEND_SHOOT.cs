using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEND_SHOOT : MonoBehaviour
{

    public static void SEND_WEAPON_SHOOT(int weaponid, Vector3 pos, Quaternion rot)
    {
        using (Packet packet = new Packet((int)ClientPackets.playerShoot))
        {
            packet.Write(1);
            packet.Write(weaponid);
            packet.Write(pos);
            packet.Write(rot);
            packet.WriteLength();

            Client.instance.udp.SendData(packet);
        }
    }

    public static void SEND_KNIFE_SHOOT(int weaponid, int type, float alternative, bool preFire = false) //Knife Attack
    {
        using (Packet packet = new Packet((int)ClientPackets.playerShoot))
        {
            packet.Write(2);
            packet.Write(weaponid);
            packet.Write(type);
            packet.Write(alternative);
            //packet.Write(preFire);
            packet.WriteLength();

            Client.instance.udp.SendData(packet);

        }
    }

    public static void SEND_GRENADE_SHOOT(int weaponid, float alternative, float[] rot = null, bool preFire = false)
    {
        using (Packet packet = new Packet((int)ClientPackets.playerShoot))
        {
            packet.Write(3);
            packet.Write(weaponid);
            packet.Write(alternative);
            packet.Write(preFire);

            if (rot != null)
            {
                packet.Write(rot[0]);
                packet.Write(rot[1]);
                packet.Write(rot[2]);
                packet.Write(rot[3]);
            }

            packet.WriteLength();

            Client.instance.udp.SendData(packet);
        }
    }
}

public class REC_SHOOT : PacketHandle
{
    public override void Handler()
    {
        int weapontype = _packet.ReadInt();

        switch (weapontype)
        {
            case 1:
                try
                {
                    int ownerid = _packet.ReadInt();
                    int weaponid = _packet.ReadInt();
                    float veloc = _packet.ReadFloat();
                    Vector3 pos = _packet.ReadVector3();
                    Quaternion rot = _packet.ReadQuaternion();
                    ///////////////////////////////////////////
                    GameManager.players[ownerid].SpawnShoot(weaponid, veloc, pos, rot);
                    var pManager = GameManager.players[ownerid].GetComponent<PlayerManager>();
                    pManager._playerWeaponLocal.StartWeaponAnim("Fire", 0, 0);
                    pManager._playerWeaponLocal.GetWeaponById(weaponid).FpvModel.GetComponent<WeaponController>().StartMuzzleFlash();
                    pManager._animController._animatorTPS.StartAnim("Fire", 0, 0);
                }
                catch { }
                break;

            case 2:
                try
                {
                    int ownerid = _packet.ReadInt();
                    int weaponid = _packet.ReadInt();
                    int type = _packet.ReadInt();
                    //bool preFire = _packet.ReadBool();
                    float alternative = _packet.ReadFloat(); //preFire == true ? _packet.ReadFloat() : 0;

                    Debug.LogError("WPID::" + weaponid);

                    var pManager = GameManager.players[ownerid].GetComponent<PlayerManager>();
                    var wp = pManager._playerWeaponsManager.GetWeaponById(weaponid);
                    //pManager._playerWeaponLocal.StartWeaponAnim("Combo1");
                    pManager._animController._animatorTPS.StartKnifeAnim(wp.TPSAnimName, type, alternative);
                    pManager._playerWeaponLocal.GetWeapon().FpvModel.GetComponent<Knife>().Attack(type, alternative);
                }
                catch { }
                break;

            case 3:
                try
                {
                    int ownerid = _packet.ReadInt();
                    int weaponid = _packet.ReadInt();
                    bool preFire = _packet.ReadBool();


                    var pManager = GameManager.players[ownerid].GetComponent<PlayerManager>();
                    var wp = pManager._playerWeaponsManager.GetWeaponById(weaponid);

                    pManager._animController._animatorTPS.StartGrenadeAnim(preFire);

                    string FPVanimName = preFire == true ? "Prefire" : "Fire";

                    pManager._playerWeaponLocal.StartWeaponAnim(FPVanimName, 0, 0);
                    if (!preFire)
                    {
                        pManager.SpawnGrenade(_packet.ReadFloat(), _packet.ReadFloat(), _packet.ReadFloat(), _packet.ReadFloat());
                        pManager._playerWeaponLocal.GetWeaponById(weaponid).TpvModel.SetActive(false);
                    }
                    else
                    {
                        pManager._soundManager.WeaponShoot(wp);
                    }
                }
                catch { }
                break;
        }

    }
}
