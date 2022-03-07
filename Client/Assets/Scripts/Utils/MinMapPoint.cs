using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinMapPoint : IsMine
{
    public Transform This;
    public Image image;
    public float distance;
    public Vector3 transforma;

    void Start()
    {
        
    }

    void Update()
    {
        if (isMine())
        {
            if (Vector3.Distance(gameObject.transform.position, This.position) > distance)
                image.gameObject.transform.position = transforma;
        }
    }
}
