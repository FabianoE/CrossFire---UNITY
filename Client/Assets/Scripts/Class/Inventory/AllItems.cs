using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllItems : MonoBehaviour
{
    public static AllItems instance;
    public InfoWeapons infoWeapons;

    public Dictionary<int, InventoryItems> inventoryAllItems = new Dictionary<int, InventoryItems>();
    public Dictionary<int, RoomListData> roomAllItems = new Dictionary<int, RoomListData>();

    public int Bag1_Primary, Bag1_Secondary, Bag1_Melee;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void SetItemsList(List<InventoryItems> items)
    {
        foreach(InventoryItems item in items)
        {
            item.weaponInfo = infoWeapons.GetWeaponById(item.itemID);
            inventoryAllItems.Add(item.inventoryID, item);
        }
    }

    public void SetRoomList(List<RoomListData> list)
    {
        foreach(var obj in list)
        {
            //if (!roomAllItems.ContainsKey(obj.Room_Id))
            //{
                roomAllItems.Add(obj.Room_Id, obj);
            //}
        }

        LobbyItemsObject.instance.SpawnRoomList();
    }
}

[Serializable]
public class InventoryItems
{
    public int inventoryID  { get; set; }
    public int itemID { get; set; }
    public WeaponsInfo weaponInfo { get; set; }
}
