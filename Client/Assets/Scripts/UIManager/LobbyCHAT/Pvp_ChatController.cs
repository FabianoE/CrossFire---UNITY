using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pvp_ChatController : MonoBehaviour
{
    public static Pvp_ChatController instance;

    [Header("Chat Lobby")]
    public GameObject PVP_ChatObject;
    public ScrollRect Scroll;
    public InputField PVP_InputText;
    public GameObject PVP_SpawnObject;
    public Transform PVP_SpawnLocal;
    [Space]
    public float HideChatTime = 0f;
    //
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (Time.time >= HideChatTime && PVP_InputText.IsActive() == false)
            PVP_ChatObject.SetActive(false);

        if (PVP_InputText.IsActive())
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Scroll.verticalNormalizedPosition = 0f;

                Debug.LogError("ACTIVE");

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                OnSendMessage();

                PVP_InputText.gameObject.SetActive(false);
                Canvas.ForceUpdateCanvases();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                PVP_ChatObject.SetActive(true);
                Scroll.verticalNormalizedPosition = 0f;

                PVP_InputText.gameObject.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Canvas.ForceUpdateCanvases();

                PVP_InputText.Select();
                PVP_InputText.ActivateInputField();
            }
        }
    }

    public void OnSendMessage()
    {
        if (PVP_InputText.text.Length > 0)
        {
            SEND_CHAT.OnSendMessage(1, PVP_InputText.text);
            PVP_InputText.text = null;
        }
    }

    public void SpawnMessage(string playerName, string msg, int chat)
    {
        PVP_ChatObject.SetActive(true);

        GameObject inst = Instantiate(PVP_SpawnObject, PVP_SpawnLocal);
        inst.GetComponent<Pvp_Chat_Class>().SetData(playerName, msg, chat);

        HideChatTime = Time.time + 10f;
    }
}
