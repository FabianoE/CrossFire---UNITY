using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteTime : MonoBehaviour
{
    public float deleteTime = 0.1f;

    void Start()
    {
        StartCoroutine(DeleteDelay());
    }

    IEnumerator DeleteDelay()
    {
        yield return new  WaitForSeconds(deleteTime);

        Destroy(gameObject);
    }
}
