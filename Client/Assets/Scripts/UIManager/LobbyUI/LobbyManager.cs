using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [HideInInspector]
    public static LobbyManager instance;
    public LobbyEnums.LobbyBoxEnum lobbyBoxEnum;
    [Space]
    public LobbyItemsObject lobbyWindows;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        Client.instance.ConnectToServer();
    }

    public void Back_Click()
    {
        if(LobbyItemsObject.instance.currentBox == LobbyEnums.LobbyBoxEnum.Room)
        {
            new SEND_LEAVE_ROOM();
        }
    }

    public void MainMenu_Buttons_Click(int item)
    {
        LobbyItemsObject.instance.SetBox((LobbyEnums.LobbyBoxEnum)item);
    }

    public void SendLogin()
    {
        SEND_AUTH_LOGIN.SendLogin(lobbyWindows.input_Login.text, lobbyWindows.input_Password.text);
        lobbyWindows.btn_Login.interactable = false;
    }

    public void Ready_Click()
    {
        new SEND_READY_ROOM();
    }

    public void LoadScene()
    {
        LobbyItemsObject.instance.loadingBox.SetActive(true);
        Client.instance.inBattle = true;
        StartCoroutine(LoadMap());
    }

    public void UnloadScene()
    {
        StartCoroutine(UnloadMap());
    }

    IEnumerator LoadMap()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Navio", LoadSceneMode.Additive);

        while (!operation.isDone)
        {
            yield return null;
        }

        Debug.LogError("CARREGADO");
        new SEND_START_BATTLE();
        LobbyItemsObject.instance.loadingBox.GetComponentInChildren<Text>().text = "AGUARDANDO JOGADORES...";
    }

    IEnumerator UnloadMap()
    {
        AsyncOperation operation = SceneManager.UnloadSceneAsync("Navio");

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
