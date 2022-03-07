using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Info_Modes Room", menuName = "Scriptable Objects/Info Rooms", order = 1)]
public class InfoModesRoom : ScriptableObject
{
    public List<ModesInfos> modeInfo;

    public List<MapsInfos> mapsInfo;

    public ModesInfos GetModeById(int id)
    {
        return modeInfo.SingleOrDefault(x => x.ModeId == id);
    }

    public List<MapsInfos> GetMapsByModeId(ModesType type)
    {
        return mapsInfo.FindAll(x => x.MapMode == type);
    }

    public MapsInfos GetMapInfoByName(string name)
    {
        return mapsInfo.FirstOrDefault(x => x.MapName == name);
    }

    public MapsInfos GetMapInfoById(int id)
    {
        return mapsInfo.FirstOrDefault(x => x.MapId == id);
    }

    public string ReturnStringModeType(ModesType type)
    {
        switch (type)
        {
            case ModesType.TDM:
                return "TDM";
            case ModesType.FFA:
                return "FFA";
            default:
                return "";
        }
    }
}

[Serializable]
public class ModesInfos
{
    public int ModeId;
    public string ModeName;
    public ModesType gameMode;
    public string TypeName;
    public int[] TypeOptions = new int[] { 40, 60, 100, 150 };
}

[Serializable]
public class MapsInfos
{
    public int MapId;
    public string MapName;
    public ModesType MapMode;
    public string MapIcon;
}

public enum ModesType
{
    TDM,
    FFA,
}
