using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LTP_Player : MonoBehaviour
{
    public static LTP_Player instance;

    public LTP_Weapons[] Weapons;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
}
