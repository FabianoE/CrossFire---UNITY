using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Packets.SERVER.Lobby
{
    class SEND_INVENTORY_DATA : Packet
    {
        public SEND_INVENTORY_DATA(Player _player) //Send Current Inventory and weapons
        {
            int id = Server.connectiondb.GetIdInventoryBag(_player.dbid, 1, "bag1_primary");
            int secondaryid = Server.connectiondb.GetIdInventoryBag(_player.dbid, 1, "bag1_secondary");
            int meleeid = Server.connectiondb.GetIdInventoryBag(_player.dbid, 1, "bag1_melee");
            int grenadeid = Server.connectiondb.GetIdInventoryBag(_player.dbid, 1, "bag1_grenade");

            int bag1 = _player.inventoryItems.Find(x => x.inventoryID == id).itemID;
            int bag1_secondary = _player.inventoryItems.Find(x => x.inventoryID == secondaryid).itemID;
            int bag1_melee = _player.inventoryItems.Find(x => x.inventoryID == meleeid).itemID;
            int bag1_grenade = _player.inventoryItems.Find(x => x.inventoryID == grenadeid).itemID;

            _player.primaryweaponid = bag1.ToString();
            _player.secondaryweapon = bag1_secondary.ToString();
            _player.tercweapon = bag1_melee.ToString();
            _player.grenadeweapon = bag1_grenade.ToString();

            _player.roomInventory.EquipWeapon(ServerObjectsManager.instance.ST_WeaponsData.GetWeaponById(int.Parse(_player.primaryweaponid)));
            _player.roomInventory.EquipWeapon(ServerObjectsManager.instance.ST_WeaponsData.GetWeaponById(int.Parse(_player.secondaryweapon)));
            _player.roomInventory.EquipWeapon(ServerObjectsManager.instance.ST_WeaponsData.GetWeaponById(int.Parse(_player.tercweapon)));
            _player.roomInventory.EquipWeapon(ServerObjectsManager.instance.ST_WeaponsData.GetWeaponById(int.Parse(_player.grenadeweapon)));

            byte[] bt = _player.inventoryItems.Serialize();
            //
            Write((int)ServerPackets.lobbyData);
            Write((int)Enums.SubLobbyData.Inventory);
            Write(1); // 1 = Weapons List, 2 = Characters List
                //
            Write(id);
            Write(bag1);
                //
            Write(secondaryid);
            Write(bag1_secondary);
                //
            Write(meleeid);
            Write(bag1_melee);
                //
            Write(grenadeid);
            Write(bag1_grenade);
                //
            Write(bt.Length);
            Write(bt);

            WriteLength();
        }

        public SEND_INVENTORY_DATA(Player _player, List<InventoryItems> characterList) //Send characters list
        {
            byte[] bt = _player.characterItems.Serialize(); Write((int)ServerPackets.lobbyData);
            Write((int)Enums.SubLobbyData.Inventory);
            Write(2); // 1 = Weapons List, 2 = Characters List

            Write(bt.Length);
            Write(bt);
            WriteLength();
        }
    }
}
