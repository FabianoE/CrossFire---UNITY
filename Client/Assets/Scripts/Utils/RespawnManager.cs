using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public Transform[] respawnPositions;
    public Transform[] respawnGRPostions;
    public Transform[] respawnFFA;

    public static RespawnManager instance;

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

    public Vector3 GetRespawn(int team)
    {
        System.Random rnd = new System.Random();
        int r = rnd.Next(0, 2);
        int f = rnd.Next(0, 1);

        if (team == 0)
            return respawnPositions[r].position;
        else if (team == 1)
            return respawnGRPostions[r].position;
        else
        {
            if(r == 0)
                return respawnPositions[r].position;
            else
                return respawnGRPostions[r].position;
        }


    }
}
