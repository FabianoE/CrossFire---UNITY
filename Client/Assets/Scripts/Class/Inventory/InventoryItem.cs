using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public Text name;
    public Image image;
    public Image bagpackimage;
    public InventoryItems weaponInfo;

    public void SetData(InventoryItems info, bool equiped = false)
    {
        name.text = info.weaponInfo.Name;
        if (!equiped)
            bagpackimage.enabled = false;

        image.sprite = Resources.Load<Sprite>("WeaponIcons/LOBBY_"+info.weaponInfo.Resource);
        weaponInfo = info;
    }

    public void EquipItem()
    {
        LobbyItemsObject.instance.EquipItemInBag(1, weaponInfo);
        new SEND_INVENTORY_EQUIP(1, weaponInfo.inventoryID, weaponInfo.weaponInfo.type);
    }
}
