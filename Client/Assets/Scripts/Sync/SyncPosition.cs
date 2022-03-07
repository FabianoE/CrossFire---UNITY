using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPosition : IsMine
{
    public PlayerManager player;
    public Vector3 oldposition = Vector3.zero;
    public float lerpTime = 1f;

    public float SendMovement = 1f;
    public bool invoked = false;
    // Update is called once per frame
    void Update()
    {
        if (isMine())
        {
            SendPosition();
        }
        else
        {
            SetPosition();
        }
    }

    void SendPosition()
    {
        if (isMine())
        {
            using (Packet packet = new Packet((int)ClientPackets.playerMovement))
            {
                packet.Write(transform.position);
                packet.Write(transform.rotation);

                packet.WriteLength();
                Client.instance.udp.SendData(packet);
            }
        }
    }

    void SetPosition()
    {
        if(gameObject.transform.position != player.position)
        {
            gameObject.transform.position = Vector3.Lerp(transform.position, player.position, lerpTime * Time.deltaTime);
        }
    }
}
