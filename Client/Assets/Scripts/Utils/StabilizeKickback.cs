using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilizeKickback : MonoBehaviour
{
    public float returnSpeed = 2.0f;
    public Transform myTransform;

    void LateUpdate()
    {
        myTransform.localRotation = Quaternion.Slerp(myTransform.localRotation, Quaternion.identity, Time.deltaTime * returnSpeed);
    }
}
