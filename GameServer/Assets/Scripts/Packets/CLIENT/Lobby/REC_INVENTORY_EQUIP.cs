using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.CLIENT.Lobby
{
    class REC_INVENTORY_EQUIP
    {
        public static void Received(Packet packet, Player player)
        {
            try
            {
                int bag = packet.ReadInt();
                int inventoryid = packet.ReadInt();
                int weapontype = packet.ReadInt();

                string bagname = weapontype == 1 ? "primary" : weapontype == 2 ? "secondary" : weapontype == 3 ? "melee" : "grenade";

                if (Server.connectiondb.UpdateItemDB(string.Format("UPDATE accounts SET bag1_{1} = {2} WHERE id = {3}", weapontype, bagname, inventoryid, player.dbid)))
                {
                    UnityEngine.Debug.LogWarning("UPDATED");
                }

                if (weapontype == 1)
                    player.primaryweaponid = player.inventoryItems.Find(x => x.inventoryID == inventoryid).itemID.ToString();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning("ERROR REC_INVENTORY_EQUIP: " + ex.Message);
            }
        }
    }
}
