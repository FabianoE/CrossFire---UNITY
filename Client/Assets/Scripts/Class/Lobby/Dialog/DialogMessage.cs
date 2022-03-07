using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogMessage : MonoBehaviour
{
    public static DialogMessage instance;

    public Text MessageText;
    public Button ConfirmBtn;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        gameObject.SetActive(false);
    }

    public void SetMessage(string message, int buttontype)
    {
        gameObject.SetActive(true);
        MessageText.text = message;

        switch (buttontype)
        {
            case 1:
                ConfirmBtn.onClick.AddListener(() => 
                {
                    gameObject.SetActive(false);
                });
                break;
        }
    }
}
