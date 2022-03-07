using GameServer.SERVER;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.CLIENT
{
    class REC_DAMAGE : Structure.PacketHandle
    {
        public override void Handler()
        {
            /*
             Perna = 2
             Pé = 3
             Peito = 4
             Braço = 5
             Cabeça = 10
             */
            try
            {
                int playerid = _packet.ReadInt();
                int targetid = _packet.ReadInt();
                int weaponid = _packet.ReadInt();
                int damagepart = _packet.ReadInt();

                int CalcDamage = ServerObjectsManager.instance.ST_WeaponsData.GetDamageByPart(weaponid, damagepart);

                var player = Server.clients[playerid];
                var target = Server.clients[targetid];


                if (player == null || target == null || player.player == null || target.player == null)
                    return;

                if (playerid == targetid || target.player.Alive == false || player.player.Alive == false)
                    return;

                int team1 = player.player.Team;
                int team2 = target.player.Team;

                if (player.player.room.mode != Enums.RoomMode.FFA && (team1 == team2))
                    return;

                if (player.player.room.blkills == 10 || player.player.room.grkills == 10)
                    return;

                target.player.Health -= CalcDamage;

                if (target.player.Health <= 0)
                {
                    target.player.Health = 0;
                    target.player.Alive = false;

                    //Add Death and Kill
                    target.player.DeathInRoom++;
                    player.player.KillInRoom++;
                    //
                    new SEND_DAMAGE(player, target, weaponid, damagepart); //SendKill
                }
                else
                {
                    new SEND_DAMAGE(player.id, target); //SendDamage
                }

                UnityEngine.Debug.LogWarning("Receive Damage" + damagepart);
            }
            catch(Exception ex) { UnityEngine.Debug.LogWarning(ex.Message); }
        }
    }

    class SEND_DAMAGE
    {
        public SEND_DAMAGE(Client player, Client targetid, int weaponid, int damagepart) //Kill
        {
            using(Packet packet = new Packet((int)ServerPackets.playerDamage))
            {
                packet.Write(1);
                packet.Write(player.id);
                packet.Write(targetid.id);
                packet.Write(weaponid);
                packet.WriteLength();

                foreach (Client client in Server.clients.Values)
                {
                    client.tcp.SendData(packet);
                }

                player.player.Kill();
                player.tcp.SendData(new SERVER.SEND_KILLMARK(player, damagepart));

                /*targetid.player.roomInventory.BagsAllWeapons.ForEach(x => {
                    x.Ammo = ServerObjectsManager.instance.ST_WeaponsData.GetWeaponById(x.ID).Ammo;
                    x.AmmoinPaint = ServerObjectsManager.instance.ST_WeaponsData.GetWeaponById(x.ID).AmmoinPaint;
                    x.MaxAmmo = ServerObjectsManager.instance.ST_WeaponsData.GetWeaponById(x.ID).MaxAmmo;
                });*/

                player.player.room.SendData(new SEND_ROOM_DATA(player.player.room, player, targetid, weaponid, damagepart));
                player.player.room.SendScore();

            }
        }

        public SEND_DAMAGE(int playerid, Client targetid) //Damage
        {
            using (Packet packet = new Packet((int)ServerPackets.playerDamage))
            {
                packet.Write(2);
                packet.Write(playerid); //ID FOR HITMARKER
                packet.Write(targetid.id);
                packet.Write(targetid.player.Health);
                packet.WriteLength();

                foreach (Client client in Server.clients.Values)
                {
                    client.tcp.SendData(packet);
                }
            }
        }
    }
}
