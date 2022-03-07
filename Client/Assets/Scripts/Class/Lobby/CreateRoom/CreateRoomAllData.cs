using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomAllData : MonoBehaviour
{
    public InfoModesRoom roomData;
    public Dropdown ListMaps;
    [Space]
    [Header("Save Config")]
    public string currentMapName;
    public int currentMode = 0;
    public int gameRules;
    public int gameObjective;
    public int gamePlayersQntd;
    [Space]
    [Header("Button Modes")]
    public Button Btn_TDM;
    public Button Btn_FFA;
    [Space]
    [Header("UI Objects")]
    public InputField roomName;
    public InputField roomPassword;
    public Image roomCurrentImage;
    [Header("TDM")]
    public Toggle[] GameRules;
    public Toggle[] Objective;
    [Header("FFA")]
    public Toggle[] FFAGameRules;
    public Toggle[] FFAObjective;
    [Space]
    public Toggle[] roomPlayers;

    private void Start()
    {
        Btn_TDM.interactable = false;
        Btn_FFA.interactable = true;

        gameRules = 0;
        gameObjective = 0;
        gamePlayersQntd = 0;

        for (int i = 0; i < GameRules.Length; i++)
        {
            GameRules[i].isOn = false;
            GameRules[i].onValueChanged.AddListener(delegate
            {
                GameRulesToggleChanged();
            });
        }

        for (int i = 0; i < FFAGameRules.Length; i++)
        {
            FFAGameRules[i].isOn = false;
            FFAGameRules[i].onValueChanged.AddListener(delegate
            {
                GameRulesToggleChanged();
            });
        }

        for (int i = 0; i < Objective.Length; i++)
        {
            Objective[i].isOn = false;
            Objective[i].onValueChanged.AddListener(delegate
            {
                GameObjectiveToggleChanged();
            });
        }

        for (int i = 0; i < FFAObjective.Length; i++)
        {
            FFAObjective[i].isOn = false;
            FFAObjective[i].onValueChanged.AddListener(delegate
            {
                GameObjectiveToggleChanged(1);
            });
        }

        for (int i = 0; i < roomPlayers.Length; i++)
        {
            roomPlayers[i].isOn = false;
            roomPlayers[i].onValueChanged.AddListener(delegate
            {
                GamePlayersToggleChanged();
            });
        }

        GameRules[0].isOn = true;
        Objective[0].isOn = true;
        roomPlayers[0].isOn = true;

        ListMaps.onValueChanged.AddListener(delegate
        {
            ListMapsChangedValue();
        });

        AddMapList(roomData.GetMapsByModeId(ModesType.TDM));

        OnBtnChangeMode(0);
    }

    public void AddMapList(List<MapsInfos> list)
    {
        ListMaps.options.Clear();
        foreach (var item in list)
        {
            ListMaps.options.Add(new Dropdown.OptionData(item.MapName));
        }
        ListMaps.value = -1;
    }

    public void SetMap(string mapname)
    {
        foreach (var item in roomData.mapsInfo)
        {
            if (item.MapName == mapname)
            {
                roomCurrentImage.sprite = Resources.Load<Sprite>(item.MapIcon);
                currentMapName = item.MapName;
                break;
            }
        }
    }

    void SetObjectivesType(int type)
    {
        switch (type)
        {
            case 0:
                if (currentMode == 0)
                {
                    Objective[0].transform.Find("Label").GetComponent<Text>().text = "60Kills";
                    Objective[1].transform.Find("Label").GetComponent<Text>().text = "100Kills";
                }
                else
                {
                    FFAObjective[0].transform.Find("Label").GetComponent<Text>().text = "60Kills";
                    FFAObjective[1].transform.Find("Label").GetComponent<Text>().text = "100Kills";
                }

                break;
            case 1:
                if (currentMode == 0)
                {
                    Objective[0].transform.Find("Label").GetComponent<Text>().text = "2 Min";
                    Objective[1].transform.Find("Label").GetComponent<Text>().text = "5 Min";
                }
                else
                {
                    FFAObjective[0].transform.Find("Label").GetComponent<Text>().text = "2 Min";
                    FFAObjective[1].transform.Find("Label").GetComponent<Text>().text = "5 Min";
                }

                break;
        }
    }

    #region Toggles Changed

    void GameRulesToggleChanged()
    {
        switch (currentMode)
        {
            case 0:
                for (int i = 0; i < GameRules.Length; i++)
                    if (GameRules[i].isOn)
                        gameRules = i;

                SetObjectivesType(gameRules);

                break;
            case 1:
                for (int i = 0; i < FFAGameRules.Length; i++)
                    if (FFAGameRules[i].isOn)
                        gameRules = i;

                SetObjectivesType(gameRules);
                break;
        }
    }

    void GameObjectiveToggleChanged(int mode = 0)
    {
        Debug.LogError("CURRENT MODE::" + currentMode);
        switch (mode)
        {
            case 0:
                for (int i = 0; i < Objective.Length; i++)
                    if (Objective[i].isOn)
                        gameObjective = i;
                break;
            case 1:
                for (int i = 0; i < FFAObjective.Length; i++)
                    if (FFAObjective[i].isOn)
                        gameObjective = i;
                break;
        }
    }

    void GamePlayersToggleChanged()
    {
        for (int i = 0; i < roomPlayers.Length; i++)
        {
            if (roomPlayers[i].isOn)
                gamePlayersQntd = i;
        }
    }

    #endregion

    void ListMapsChangedValue()
    {
        Debug.LogError(ListMaps.options[ListMaps.value].text);
        SetMap(ListMaps.options[ListMaps.value].text);
    }

    #region Event Button Click
    public void OnBtnChangeMode(int i)
    {
        switch (i)
        {
            case 0: //TDM
                Btn_TDM.interactable = false;
                Btn_FFA.interactable = true;

                currentMode = 0;
                gameObjective = 0;
                gameRules = 0;

                GameRules[0].isOn = true;
                Objective[0].isOn = true;

                GameRules[0].group.gameObject.SetActive(true);
                FFAGameRules[0].group.gameObject.SetActive(false);

                Objective[0].group.gameObject.SetActive(true);
                FFAObjective[0].group.gameObject.SetActive(false);

                AddMapList(roomData.GetMapsByModeId(ModesType.TDM));
                break;
            case 1: //FFA
                Btn_FFA.interactable = false;
                Btn_TDM.interactable = true;

                currentMode = 1;
                gameObjective = 0;
                gameRules = 0;

                FFAGameRules[0].isOn = true;
                FFAObjective[0].isOn = true;

                GameRules[0].group.gameObject.SetActive(false);
                FFAGameRules[0].group.gameObject.SetActive(true);

                Objective[0].group.gameObject.SetActive(false);
                FFAObjective[0].group.gameObject.SetActive(true);

                AddMapList(roomData.GetMapsByModeId(ModesType.FFA));
                break;
        }
    }
    
    public void CreateRoom_Btn()
    {
        string name = roomName.text;
        int roomMode = currentMode;
        int roomMap = roomData.GetMapInfoByName(currentMapName).MapId;
        int roomType = gameRules;
        int roomTypeOption = gameObjective;
        int maxPlayer = gamePlayersQntd;

        new SEND_CREATE_ROOM(name, roomMode, roomMap, roomType, roomTypeOption, maxPlayer, 0);
    }
    #endregion
}

