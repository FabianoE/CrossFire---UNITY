using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player
{
    public int id;
    public string username;
    public int weapon = 1;
    public int Health = 100;
    public int Team = -1;
    public int exp = 0;

    //Kills Info
    public int KillInRoom = 0;
    public int DeathInRoom = 0;
    public int LastKills = 0;
    //
    public string ping;
    //
    public Vector3 position;
    public Quaternion rotation;
}
