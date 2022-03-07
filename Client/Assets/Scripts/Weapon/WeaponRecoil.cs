using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    public Vector3[] Recoils;
    int current = 0;
    public float speed = 1f;
    public float timen = 0f;
    public float returntime = 0f;

    public float recoilm = 1f;

    public float returnSpeed = 1f;
    public Transform myTransform;

    public GameObject gm;
    public Vector3 rot;


    void Update()
    {
        //if(timen < Time.time)
       // {
        //    rot = Vector3.zero;
        //}
    }

    private void LateUpdate()
    {
        if(timen < Time.time)
        rot = Vector3.Slerp(rot, Vector3.zero, 2 * Time.deltaTime);
    }

    public void Fire()
    {
        if (current >= Recoils.Length)
            current = 0;

        //gm.transform.localEulerAngles += Vector3.Lerp(gm.transform.localPosition, Recoils[current], speed);
        rot = Vector3.Slerp(rot, Recoils[current], speed * Time.fixedDeltaTime);
        rot.z = 0;
        gm.transform.localRotation = Quaternion.Euler(rot);
        timen = Time.time + recoilm;
        current++;
    }
}
