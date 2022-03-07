using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_ChatController : MonoBehaviour
{
    public static Lobby_ChatController instance;

    [Header("Chat Lobby")]
    public InputField Lobby_InputText;
    public GameObject Lobby_SpawnObject;
    public Transform Lobby_SpawnLocal;

    private void Awake()
    {
        instance = this;
    }

    public void OnSendMessage()
    {
        if (Lobby_InputText.text.Length > 0)
        {
            if (!CheckChatCommands(Lobby_InputText.text))
                SEND_CHAT.OnSendMessage(1, Lobby_InputText.text);

            Lobby_InputText.text = null;
        }
    }

    public void SpawnMessage(string playerName, string msg)
    {
        GameObject inst = Instantiate(Lobby_SpawnObject, Lobby_SpawnLocal);
        inst.GetComponent<Chat_Class>().SetText(playerName, msg);
    }

    public void ClearChatTexts()
    {
        for (int i = 0; i < Lobby_SpawnLocal.childCount; i++)
            Destroy(Lobby_SpawnLocal.GetChild(i).gameObject);
    }

    bool CheckChatCommands(string text)
    {
        switch (text)
        {
            case "/Clear":
            case "/clear":
                ClearChatTexts();
                break;
            case "/":
            case "/help":
                SpawnMessage("SYSTEM", "All Commands");
                SpawnMessage(null, "/clear");
                break;
            default:
                return false;
        }

        return true;
    }
}
