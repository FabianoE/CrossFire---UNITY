using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

class REC_BATTLE_START : MonoBehaviour
{
    public static void Received(Packet packet)
    {
        LobbyManager.instance.LoadScene();
    }
}

class REC_BATTLE_STOP : MonoBehaviour
{
    public static void Received(Packet packet)
    {
        int blscore = packet.ReadInt();
        int grscore = packet.ReadInt();
        int wteam = packet.ReadInt();
        int roommode = packet.ReadInt();
        int packetlenght = packet.ReadInt();
        byte[] bt = packet.ReadBytes(packetlenght);


        LobbyItemsObject.instance.loadingBox.SetActive(false);
        LobbyItemsObject.instance.gameObject.SetActive(true);
        if (Client.instance.inBattle)
            LobbyItemsObject.instance.EndBattle(bt, roommode, wteam, blscore, grscore);

        LobbyManager.instance.UnloadScene();

        foreach (var obj in GameManager.players.Values)
            obj.GetComponent<IsMine>().Delete();

        GameManager.players.Clear();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Client.instance.inBattle = false;
    }
}
