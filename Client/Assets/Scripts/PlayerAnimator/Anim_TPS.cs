using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_TPS : MonoBehaviour
{
    private Animator _animator;

    public float LeggyBoddyDir;
    public float UpperBodyDir;
    public Transform[] LeggyBoddy;
    public Transform[] UpperBoddy;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        foreach(Transform u in LeggyBoddy)
        {
            u.RotateAround(u.position, transform.up, LeggyBoddyDir);
        }

        foreach(Transform u in UpperBoddy)
        {
            u.RotateAround(u.position, transform.right, UpperBodyDir);
        }
    }

    public void StartAnim(string name)
    {
        _animator.Play(name);
    }

    public void StartAnim(string name, int i, int j)
    {
        _animator.Play(name, i, j);
    }

    public void SetIntAnim(string name, int value)
    {
        _animator.SetInteger(name, value);
    }

    public void SetBoolAnim(string name, bool value)
    {
        _animator.SetBool(name, value);
    }

    public void StartKnifeAnim(string name, int type, float alternative)
    {
        Debug.LogError("KNIFE::" + name);
        switch (type)
        {
            case 1:
                if(alternative > 0)
                {
                    _animator.Play(name+"_Combo2");
                }
                else
                {
                    _animator.Play(name+"_Combo1");
                }
                break;
            case 2:
                    _animator.Play(name+"_BigShot");
                break;;
        }
    }

    public void StartGrenadeAnim(bool preFire = false)
    {
        if (preFire)
        {
            _animator.Play("Grenade_Prefire");
            return;
        }

        _animator.Play("Grenade_Fire");
    }

    public void SetWalkAnim(bool walking, int walk, int leggyrotation, bool lower)
    {
        string Type = "Walk";

        if (walking)
        {
            _animator.SetBool("Walk", true);

            switch(walk)
            {
                case 1:
                    Type = "Forward";
                    break;
                case 2:
                    Type = "Backward";
                    break;
                case 11:
                    Type = "Right";
                    break;
                case 12:
                    Type = "Left";
                    break;
            }

            if (lower)
                Type = Type + "_Lower";

            if (Type == "Forward")
            {
                if (leggyrotation > 0)
                {
                    LeggyBoddyDir = leggyrotation == 20 ? 40 : -40;
                }
                else
                    LeggyBoddyDir = 0;
            }
            else if (Type == "Backward") 
            {
                if (leggyrotation > 0)
                {
                    LeggyBoddyDir = leggyrotation == 20 ? -40 : 40;
                }
                else
                    LeggyBoddyDir = 0;
            }
            else
                LeggyBoddyDir = 0;

            _animator.SetBool("Lower", lower);

            _animator.Play(Type+"_M4");
        }
        else
        {
            _animator.SetBool("Walk", false);
            _animator.SetBool("Lower", lower);
        }
    }

    public void SetUpperBody(string upperbody)
    {
        UpperBodyDir = float.Parse(upperbody);
    }

}
