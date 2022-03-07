using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GameServer.Packets.CLIENT
{
    class REC_SHOOT : Structure.PacketHandle
    {
        public override void Handler()
        {
            int weaponType = _packet.ReadInt();


            switch (weaponType)
            {
                case 1: //WeaponShoot
                    try
                    {
                        int weaponid = _packet.ReadInt();
                        Vector3 pos = _packet.ReadVector3();
                        Quaternion rot = _packet.ReadQuaternion();

                        if (_player.getWeaponEquiped().AmmoinPaint <= 0 || _player.isReloading == true)
                            return;

                        using (Packet packet = new Packet((int)ServerPackets.playerShoot))
                        {
                            packet.Write(1);
                            packet.Write(_player.id);
                            packet.Write(weaponid);
                            packet.Write(200f);
                            packet.Write(pos);
                            packet.Write(rot);
                            packet.WriteLength();

                            _player.room.SendData(packet);


                            _player.getWeaponEquiped().AmmoinPaint--;
                        }
                    }
                    catch { }
                    break;
                ///////////////////////////////////////////////////////////
                case 2://KnifeAttack
                    try
                    {
                        int weaponid = _packet.ReadInt();
                        int type = _packet.ReadInt();
                        float alternativetime = _packet.ReadFloat();
                        //bool preFire = _packet.ReadBool();

                        using (Packet packet = new Packet((int)ServerPackets.playerShoot))
                        {
                            packet.Write(2);
                            packet.Write(_player.id);
                            packet.Write(weaponid);
                            //packet.Write(preFire);
                            //if (!preFire)
                            //{
                            packet.Write(type);
                            packet.Write(alternativetime);
                            //}
                            packet.WriteLength();

                            _player.room.SendData(packet);
                        }
                    }
                    catch { }
                    break;

                case 3:
                    try
                    {
                        //UnityEngine.Debug.LogError("QNT " + _player.getWeaponEquiped().AmmoinPaint);
                        if (_player.getWeaponEquiped().AmmoinPaint <= 0) // SE NÃO TIVER GRANADA DISPONIVEL
                            return;

                        int weaponid = _packet.ReadInt();
                        float alternativetime = _packet.ReadFloat();
                        bool preFire = _packet.ReadBool();
                        Vector3 pos = _packet.ReadVector3();
                        Quaternion rot = _packet.ReadQuaternion();

                        using (Packet packet = new Packet((int)ServerPackets.playerShoot))
                        {
                            packet.Write(3);
                            packet.Write(_player.id);
                            packet.Write(weaponid);
                            packet.Write(preFire);
                            packet.Write(pos);
                            packet.Write(rot);

                            packet.WriteLength();

                            _player.room.SendData(packet);

                            if (preFire == false) //Se não for prefire vai retirar uma granada
                                _player.getWeaponEquiped().AmmoinPaint--;
                        }
                    }
                    catch { }
                    break;
                    ///////////////////////////////////////////////////////////
            }

        }
    }
}
