using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEND_INVENTORY_EQUIP : MonoBehaviour
{
    public SEND_INVENTORY_EQUIP(int bag, int inventoryId, WType weapontype)
    {
        int weapontp = returnIntWeaponType(weapontype);

        using (Packet packet = new Packet((int)ClientPackets.lobbyData))
        {
            packet.Write((int)LobbyEnums.SubLobbyData.InventoryEquip);
            packet.Write(bag);
            packet.Write(inventoryId);
            packet.Write(weapontp);
            packet.WriteLength();

            Client.instance.tcp.SendData(packet);
        }
    }

    int returnIntWeaponType(WType wType)
    {
        switch (wType)
        {
            case WType.Rifle:
            case WType.Sniper:
                return 1;
            case WType.Pistol:
                return 2;
            case WType.Knife:
                return 3;
            default:
                return 0;
        }
    }
}
