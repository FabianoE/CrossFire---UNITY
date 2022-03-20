using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class REC_INVENTORY_DATA : MonoBehaviour
{
    public static void Received(Packet packet)
    {
        int invType = packet.ReadInt(); // 1 = Inventory Weapons || 2 = Inventory Characters(incomplete)

        if (invType == 2) //If inventory data == inventory characters
            return;

        int iventoryid = packet.ReadInt(); //Primary
        int itemid = packet.ReadInt();

        int iventoryid2 = packet.ReadInt(); //Secondary
        int itemid2 = packet.ReadInt();

        int inventoryid3 = packet.ReadInt(); //Melee
        int itemid3 = packet.ReadInt();

        int inventoryid4 = packet.ReadInt(); //Grenade
        int itemid4 = packet.ReadInt();

        int bt = packet.ReadInt();
        List<InventoryItems> list = packet.ReadBytes(bt).DeserializeObject<List<InventoryItems>>();

        AllItems.instance.SetItemsList(list);

        AllItems.instance.Bag1_Primary = iventoryid;
        AllItems.instance.Bag1_Secondary = iventoryid2;
        AllItems.instance.Bag1_Melee = inventoryid3;

        LobbyItemsObject.instance.SetBagItems(0, itemid);//Primary
        LobbyItemsObject.instance.SetBagItems(1, itemid2);//Secondary
        LobbyItemsObject.instance.SetBagItems(2, itemid3);//Melee(Knife)

        LTP_Manager.instance.EnableWeapon(itemid);
    }
}
