using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    public Anim_TPS _animatorTPS;

    public void CheckWalk(float x, float y, string upperbody, bool lower = false, bool shift = false)
    {
        if (x != 0 || y != 0)
        {
            int walks = 0;
            int leggys = 0;

            if(y > 0)
            {
                walks = 1;
                if (x > 0)
                    leggys = 20;
                else if (x < 0)
                    leggys = 10;
            }
            if(y < 0)
            {
                    walks = 2;
                if (x > 0)
                    leggys = 20;
                else if (x < 0)
                    leggys = 10;
            }
            if(y == 0)
            {
                    walks = 11;
                if(x > 0)
                    walks = 11;
                else if(x < 0)
                    walks = 12;
            }

            SendWalk(true, upperbody, walks, leggys, lower, shift);
        }
        else
        {
            SendWalk(false, upperbody, 0, 0 ,lower, shift);
        }
    }

    #region SendAnim

    void SendWalk(bool walking, string upperboddy, int walk, int leggyrotation, bool lower = false, bool shift = false)
    {
        using (Packet packet = new Packet((int)6))
        {
            packet.Write(Client.instance.myId);
            packet.Write(105);
            packet.Write(upperboddy);
            packet.Write(walk);
            packet.Write(leggyrotation);
            packet.Write(walking);
            packet.Write(lower);
            packet.Write(shift);
            packet.WriteLength();

            Client.instance.udp.SendData(packet);
        }
    }

    #endregion

    #region 

    public void SetWalk(string upperbody, bool walking, int walk, int leggybody, bool lower)
    {
        _animatorTPS.SetWalkAnim(walking, walk, leggybody, lower);
        _animatorTPS.SetUpperBody(upperbody);

    }

    #endregion
}
