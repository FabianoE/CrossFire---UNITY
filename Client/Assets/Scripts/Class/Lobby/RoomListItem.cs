using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    public int Room_id;
    public string Room_Name;
    public string Room_Mode;
    public string Room_Map;
    public string Room_Players;
    [Space]
    public Text room_id;
    public Text room_name;
    public Text room_mode;
    public Text room_map;
    public Text room_players;
    [Space]
    public float lastClick = 0;
    public float intervel = 0.15f;
    public void SetData(int roomid, string name, string mode, string map, string players)
    {
        room_id.text = roomid.ToString();
        room_name.text = name;
        room_mode.text = mode;
        room_map.text = map;
        room_players.text = players.ToString();
        //
        Room_id = roomid;
        Room_Name = name;
        Room_Mode = mode;
        Room_Map = map;
        Room_Players = players;

        SetTextsData();
    }

    void SetTextsData()
    {
        MapsInfos info = LobbyItemsObject.instance.MapsallData.GetMapInfoById(int.Parse(Room_Map));

        room_map.text = info.MapName;
        room_mode.text = LobbyItemsObject.instance.MapsallData.ReturnStringModeType(info.MapMode);
    }

    public void EnterRoom()
    {
        Debug.LogError("Double Clickk");
        if ((lastClick + intervel) > Time.time)
        {
            Debug.LogError("Double Click");
            new SEND_ENTER_ROOM(Room_id);
        }
        lastClick = Time.time;
    }

    public void OnMouseHover()
    {
        MapsInfos info = LobbyItemsObject.instance.MapsallData.GetMapInfoById(int.Parse(Room_Map));


        LobbyItemsObject.instance.LRoom_Image.color = Color.white;
        LobbyItemsObject.instance.LRoom_Image.sprite = Resources.Load<Sprite>(info.MapIcon);
        LobbyItemsObject.instance.LRoom_Map.text = info.MapName;
        LobbyItemsObject.instance.LRoom_Mode.text = LobbyItemsObject.instance.MapsallData.ReturnStringModeType(info.MapMode);
        LobbyItemsObject.instance.LRoom_Name.text = room_name.text;
        LobbyItemsObject.instance.LRoom_Players.text = room_players.text;
    }

    public void OnMouseLeave()
    {
        LobbyItemsObject.instance.LRoom_Image.color = Color.gray;
        LobbyItemsObject.instance.LRoom_Image.sprite = null;
        LobbyItemsObject.instance.LRoom_Map.text = "MAP NAME";
        LobbyItemsObject.instance.LRoom_Mode.text = "MODE";
        LobbyItemsObject.instance.LRoom_Name.text = "ROOM NAME";
        LobbyItemsObject.instance.LRoom_Players.text = "PLAYERS";
    }

}
