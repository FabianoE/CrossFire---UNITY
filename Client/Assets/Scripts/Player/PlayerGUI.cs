using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : IsMine
{
    public TMP_Text playerName;

    private void Start()
    {
        if (isMine())
            playerName.gameObject.SetActive(false);

        playerName.text = _client.username;
    }
}
