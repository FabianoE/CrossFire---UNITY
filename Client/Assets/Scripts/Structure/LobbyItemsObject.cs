using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LobbyItemsObject : MonoBehaviour
{
    public LobbyEnums.LobbyBoxEnum currentBox = LobbyEnums.LobbyBoxEnum.Lobby;
    public static LobbyItemsObject instance;

    public InfoModesRoom MapsallData;

    [Header("LOGIN WINDOW")]
    public InputField input_Login;
    public InputField input_Password;
    public Button btn_Login;
    [Space(10)]
    [Header("Buttons MainMenu")]
    public Button btn_lobby;
    public Button btn_inventory;
    public Button btn_back;
    [Space(5)]
    [Header("Box Lobby")]
    public GameObject box_login;
    public GameObject box_lobby;
    public GameObject box_room;
    public GameObject box_inventory;
    public GameObject box_createroom;
    [Space(5)]
    [Header("Inventory Objects")]
    public GameObject[] bagItems;
    public GameObject itemToSpawnInventory;
    public Transform localSpawnItemsInventory;
    [Space(5)]
    [Header("Player Infos")]
    public Text player_name;
    public Text player_cash;
    public Text player_gold;
    public Text player_patent_porcentage;
    public Image player_patent_image;
    public Slider player_patent_slider;
    [Space(5)]
    [Header("Inventory Buttons")]
    public Button btn_AllWeapons;
    public Button btn_RifleWeapons;
    public Button btn_SniperWeapons;
    [Space]
    public Button btn_Bag1;
    public Button btn_Bag2;
    public Button btn_Bag3;
    [Space(5)]
    [Header("Lobby Room")]
    public RoomSlotItem[] RoomAllSlots;
    public Transform localSpawnItemRoomList;
    public GameObject itemToSpawnRoomList;
    [Space]
    public Text Room_Name;
    public Text Room_Mode;
    public Text Room_Map_Name;
    public Text Room_Type;
    public Text Room_Type_Result;
    public Text Room_Time;
    public Image Room_Map_Image;
    public Button Room_Btn_Ready;
    [Space(5)]
    [Header("Lobby Left Room Info")]
    public Image LRoom_Image;
    public Text LRoom_Name;
    public Text LRoom_Mode;
    public Text LRoom_Map;
    public Text LRoom_Players;

    [Space(5)]
    [Header("Lobby CreateRoom - In Script(CreateRoomAllData)")]
    public CreateRoomAllData createRoom;
    [Space(5)]
    [Header("Loading")]
    public GameObject loadingBox;
    [Space(5)]
    [Header("End Battle")]
    public GameObject Panel_EndBattle;
    public GameObject Box_EndBattle; 
    public GameObject Box_FFA_EndBattle;
    public GameObject[] LIST_MYTEAM_EndBattle;
    public GameObject[] LIST_OTHERTEAM_EndBattle;
    public GameObject[] LIST_FFA_EndBattle;
    [Space]
    [Header("End Battle - Edit. Objects")]
    [Header("MyTeam")]
    public Image MyTeam_BG;
    public Image MyTeam_Effect;
    public Text MyTeam_Name;
    public Text MyTeam_Score;
    public Text MyTeam_Win;
    [Header("OtherTeam")]
    public Text OtherTeam_Name;
    public Text OtherTeam_Score;
    public Text OtherTeam_Win;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetBox(LobbyEnums.LobbyBoxEnum.Lobby);
    }

    public void SetBox(LobbyEnums.LobbyBoxEnum item)
    {
        EnableAllBtns();

        switch (item)
        {
            case LobbyEnums.LobbyBoxEnum.Lobby:
                if(currentBox == LobbyEnums.LobbyBoxEnum.Room)
                {
                    SetBox(LobbyEnums.LobbyBoxEnum.Room);
                    return;
                }

                box_inventory.SetActive(false);
                box_lobby.SetActive(true);
                box_room.SetActive(false);
                box_createroom.SetActive(false);

                btn_lobby.interactable = false;
                btn_back.enabled = false;
                btn_lobby.GetComponentInChildren<Text>().text = "Lobby";

                //Disable or enable LOBBY BOTTOM
                Lobby_Bottom_Manager.instance.SetStatus(true);
                break;

            case LobbyEnums.LobbyBoxEnum.Inventory:
                box_lobby.SetActive(false);
                box_inventory.SetActive(true);
                box_room.SetActive(false);
                box_createroom.SetActive(false);
                btn_back.enabled = false;
                btn_inventory.interactable = false;
                SpawnItemInventory();

                //Disable or enable LOBBY BOTTOM
                Lobby_Bottom_Manager.instance.SetStatus(false);
                break;

            case LobbyEnums.LobbyBoxEnum.Room:
                box_lobby.SetActive(false);
                box_inventory.SetActive(false);
                box_room.SetActive(true);
                box_createroom.SetActive(false);

                currentBox = item;
                btn_lobby.GetComponentInChildren<Text>().text = "ROOM";
                btn_back.enabled = true;
                btn_lobby.interactable = false;

                //Disable or enable LOBBY BOTTOM
                Lobby_Bottom_Manager.instance.SetStatus(true);
                break;

            case LobbyEnums.LobbyBoxEnum.CreateRoom:
                box_inventory.SetActive(false);
                box_lobby.SetActive(false);
                box_room.SetActive(false);
                box_createroom.SetActive(true);

                btn_back.enabled = false;
                btn_lobby.GetComponentInChildren<Text>().text = "LOBBY";

                //Disable or enable LOBBY BOTTOM
                Lobby_Bottom_Manager.instance.SetStatus(false);
                break;
        }
    }

    public void SetBagItems(int bag, int itemid)
    {
        Debug.LogError("ITEMID: "+itemid + " BAG: " + bag);
        GameObject bagobj = bagItems[bag];

        WeaponsInfo info = AllItems.instance.infoWeapons.GetWeaponById(itemid);

        bagobj.GetComponent<BagItem>().SetData(info);
    }

    public void SetRoomData(string name, int mode, int map, int respawn, int type, int typeoption)
    {
        MapsInfos info = MapsallData.GetMapInfoById(map);
        Room_Name.text = name;
        Room_Mode.text = MapsallData.ReturnStringModeType(info.MapMode);
        Room_Map_Image.sprite = Resources.Load<Sprite>(info.MapIcon);
        Room_Map_Name.text = info.MapName;
        Room_Type.text = "Time";
        Room_Type_Result.text = "10 Minutes";
        Room_Time.text = "Respawn Time: " + respawn.ToString() + " Sec";
    }

    public void SetPlayerInfo(string name, int cash, int gold, int exp, int kills, int deaths)
    {
        player_name.text = name;
        player_cash.text = cash.ToString();
        player_gold.text = gold.ToString();
        player_patent_image.sprite = Resources.Load<Sprite>("UI/Class/BigClass_" + GetPatent.ReturnPatentByExp(exp));
        player_patent_slider.maxValue = GetPatent.ReturnMaxExpPatentByExp(exp);
        player_patent_slider.value = exp;
        player_patent_porcentage.text = GetPatent.ReturnPorcentage(exp).ToString();
    }

    public void SpawnItemInventory()
    {
        for(int i = 0; i < localSpawnItemsInventory.childCount; i++)
        {
            Destroy(localSpawnItemsInventory.GetChild(i).gameObject);
        }

        foreach (var item in AllItems.instance.inventoryAllItems.Values.OrderBy(x => x.weaponInfo.type))
        {
            bool equiped = false;
            //WeaponsInfo info = AllItems.instance.infoWeapons.GetWeaponById(item.itemID);

            if (AllItems.instance.Bag1_Primary == item.inventoryID || AllItems.instance.Bag1_Secondary == item.inventoryID || AllItems.instance.Bag1_Melee == item.inventoryID)
                equiped = true;

            GameObject obj = Instantiate(itemToSpawnInventory, localSpawnItemsInventory);
            obj.GetComponent<InventoryItem>().SetData(item, equiped);
        }
    }

    public void SpawnRoomList()
    {
        for(int i = 0; i < localSpawnItemRoomList.childCount; i++)
        {
            Destroy(localSpawnItemRoomList.GetChild(i).gameObject);
        }

        foreach(var item in AllItems.instance.roomAllItems.Values.OrderBy(x => x.Room_Id))
        {
            GameObject obj = Instantiate(itemToSpawnRoomList, localSpawnItemRoomList);
            obj.GetComponent<RoomListItem>().SetData(item.Room_Id, item.Room_Name, item.Room_Mode.ToString(), item.Room_Map.ToString(), item.Room_Players);
        }

        AllItems.instance.roomAllItems.Clear();
    }

    public void EquipItemInBag(int bag, InventoryItems item)
    {
        if (item.inventoryID == AllItems.instance.Bag1_Primary || item.inventoryID == AllItems.instance.Bag1_Secondary || item.inventoryID == AllItems.instance.Bag1_Melee)
            return;

        switch (item.weaponInfo.type)
        {
            case WType.Rifle:
            case WType.Sniper:
                try
                {
                    int current = AllItems.instance.Bag1_Primary;
                    returnInventoryById(current).GetComponent<InventoryItem>().bagpackimage.enabled = false;

                    AllItems.instance.Bag1_Primary = item.inventoryID;
                    returnInventoryById(AllItems.instance.Bag1_Primary).GetComponent<InventoryItem>().bagpackimage.enabled = true;
                    //
                    bagItems[0].GetComponent<BagItem>().SetData(item.weaponInfo);
                    LTP_Manager.instance.EnableWeapon(item.itemID);
                }
                catch { }
                break;
            ///////////////////////////////////////////////////////
            case WType.Pistol:
                try
                {
                    int current = AllItems.instance.Bag1_Secondary;
                    returnInventoryById(current).GetComponent<InventoryItem>().bagpackimage.enabled = false;

                    AllItems.instance.Bag1_Secondary = item.inventoryID;
                    returnInventoryById(AllItems.instance.Bag1_Secondary).GetComponent<InventoryItem>().bagpackimage.enabled = true;
                    //
                    bagItems[1].GetComponent<BagItem>().SetData(item.weaponInfo);
                }
                catch { }
                break;
            //////////////////////////////////////////////////////
            case WType.Knife:
                try
                {
                    int current = AllItems.instance.Bag1_Melee;
                    returnInventoryById(current).GetComponent<InventoryItem>().bagpackimage.enabled = false;

                    AllItems.instance.Bag1_Melee = item.inventoryID;
                    returnInventoryById(AllItems.instance.Bag1_Melee).GetComponent<InventoryItem>().bagpackimage.enabled = true;
                    //
                    bagItems[2].GetComponent<BagItem>().SetData(item.weaponInfo);
                }
                catch { }
                break;
        }
    }

    public void EndBattle(byte[] bt, int roommode, int winnteam, int blscore, int grscore)
    {
        List<Player> list = bt.DeserializeObject<List<Player>>();

        Panel_EndBattle.SetActive(true);

        Color MyTeamColorLIST = Client.instance.myTeam == 1 ? new Color32(3, 74, 120, 255) : new Color32(120, 84, 3, 255);

        if (winnteam == Client.instance.myTeam)
        {
            MyTeam_Win.text = "WIN";
            MyTeam_Win.color = Color.yellow;
            OtherTeam_Win.text = "LOSE";
            OtherTeam_Win.color = Color.gray;
        }
        else
        {
            MyTeam_Win.text = "LOSE";
            MyTeam_Win.color = Color.gray;
            OtherTeam_Win.text = "WIN";
            OtherTeam_Win.color = Color.yellow;
        }

        if (Client.instance.myTeam == 0) //BL
        {
            MyTeam_Name.text = "BLACK LIST";
            OtherTeam_Name.text = "GLOBAL RISK";
            MyTeam_Score.text = blscore.ToString();
            OtherTeam_Score.text = grscore.ToString();
        }
        else //GR
        {
            MyTeam_Name.text = "GLOBAL RISK";
            OtherTeam_Name.text = "BLACK LIST";
            MyTeam_Score.text = grscore.ToString();
            OtherTeam_Score.text = blscore.ToString();
        }

        MyTeam_BG.color = Client.instance.myTeam == 1 ? new Color32(1, 41, 67, 255) : new Color32(75, 41, 19, 255);
        MyTeam_Effect.color = Client.instance.myTeam == 1 ? new Color32(0, 162, 255, 255) : new Color32(255, 69, 0, 255);

        if (roommode != 1) //Not FFA
        {
            Box_EndBattle.SetActive(true);
            Box_FFA_EndBattle.SetActive(false);

            List<Player> MyTeam = new List<Player>();
            List<Player> OtherTeam = new List<Player>();

            int p1 = 0;
            int p2 = 0;
            for(int i = 0; i < 8; i++)
            {
                LIST_MYTEAM_EndBattle[i].SetActive(false);
                LIST_OTHERTEAM_EndBattle[i].SetActive(false);
            }

            foreach(var obj in list)
            {
                if (obj.Team == Client.instance.myTeam)
                    MyTeam.Add(obj);
                else
                    OtherTeam.Add(obj);
            }

            foreach(var obj in MyTeam.OrderByDescending(x => x.KillInRoom).OrderBy(x => x.DeathInRoom))
            {
                GameObject gameobj = LIST_MYTEAM_EndBattle[p1]; gameobj.SetActive(true);

                gameobj.GetComponent<Image>().color = obj.Team == 1 ? new Color32(3, 74, 120, 255) : new Color32(120, 84, 3, 255);
                
                gameobj.transform.Find("Text_PlayerName ").GetComponent<Text>().text = obj.username;
                gameobj.transform.Find("Text_UserKills").GetComponent<Text>().text = obj.KillInRoom.ToString();
                gameobj.transform.Find("Text_UserDeaths").GetComponent<Text>().text = obj.DeathInRoom.ToString();

                p1++;
            }

            foreach (var obj in OtherTeam.OrderByDescending(x => x.KillInRoom).OrderBy(x => x.DeathInRoom))
            {
                GameObject gameobj = LIST_OTHERTEAM_EndBattle[p2]; gameobj.SetActive(true);

                gameobj.transform.Find("Text_PlayerName ").GetComponent<Text>().text = obj.username;
                gameobj.transform.Find("Text_UserKills").GetComponent<Text>().text = obj.KillInRoom.ToString();
                gameobj.transform.Find("Text_UserDeaths").GetComponent<Text>().text = obj.DeathInRoom.ToString();

                p2++;
            }
        }
        else
        {
            for (int i = 0; i < 16; i++)
                LIST_FFA_EndBattle[i].SetActive(false);

            Box_EndBattle.SetActive(false);
            Box_FFA_EndBattle.SetActive(true);

            int p1 = 0;

            foreach (var obj in list.OrderByDescending(x => x.KillInRoom).OrderBy(x => x.DeathInRoom))
            {
                GameObject gameobj = LIST_FFA_EndBattle[p1]; gameobj.SetActive(true);

                gameobj.transform.Find("Text_PlayerName").GetComponent<Text>().text = obj.username;
                gameobj.transform.Find("Text_UserKills").GetComponent<Text>().text = obj.KillInRoom.ToString();
                gameobj.transform.Find("Text_UserDeaths").GetComponent<Text>().text = obj.DeathInRoom.ToString();

                p1++;
            }
        }
    }

    public Transform returnInventoryById(int inventoryId)
    {
        Transform obj = null;

        for (int i = 0; i < localSpawnItemsInventory.childCount; i++)
        {
            if(localSpawnItemsInventory.GetChild(i).GetComponent<InventoryItem>().weaponInfo.inventoryID == inventoryId)
            {
                obj = localSpawnItemsInventory.GetChild(i);
            }
        }

        return obj;
    }

    void EnableAllBtns()
    {
        btn_inventory.interactable = true;
        btn_lobby.interactable = true;
    }
}

