using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerObjectsManager : MonoBehaviour
{
    public static ServerObjectsManager instance;

    public ST_WeaponsData ST_WeaponsData;

    private void Start()
    {
        Application.targetFrameRate = 60;
        instance = this;
        Debug.LogWarning("ServerObjectsManager -- STARTED");
    }
}
