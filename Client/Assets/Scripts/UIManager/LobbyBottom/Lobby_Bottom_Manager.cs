using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_Bottom_Manager : MonoBehaviour
{
    public static Lobby_Bottom_Manager instance;
    public GameObject LobbyBottom;
    public GameObject ChatObj;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void SetStatus(bool enabled)
    {
        LobbyBottom.SetActive(enabled);
    }
}
