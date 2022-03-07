using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class REC_ROOM_LIST : MonoBehaviour
{
    public static void Received(Packet packet)
    {
        int bytelenght = packet.ReadInt();

        List<RoomListData> list = packet.ReadBytes(bytelenght).DeserializeObject<List<RoomListData>>();

        AllItems.instance.SetRoomList(list);
    }
}
