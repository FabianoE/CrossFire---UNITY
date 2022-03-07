using AssetBundleManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Set Name")]
    public GameObject startMenu;
    public InputField usernameField;
    [Space]
    [Header("Download Assets")]
    public GameObject DownloadMenu;
    public Text DownloadText;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void ConnectToServer()
    {
        StartCoroutine(PreDownload());
        DownloadMenu.SetActive(true);
    }

    IEnumerator PreDownload()
    {
        DownloadText.text = "Carregando Recursos";
        yield return StartCoroutine(ABManager.instance.DownloadFilesGameObj());
        DownloadText.text = "Carregando Mapa";
        //yield return StartCoroutine(ABManager.instance.DownloadMap("Navio"));
        DownloadText.text = "Carregando Mapa";
        //yield return Instantiate<GameObject>(ABManager.instance.gameMapsList["Navio"]);
        DownloadText.text = "Pronto.";
        startMenu.SetActive(false);
        usernameField.interactable = false;
        Client.instance.ConnectToServer();
        DownloadMenu.SetActive(false);
    }
}
