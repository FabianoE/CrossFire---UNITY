using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketHandle : MonoBehaviour
{
    public Packet _packet;
    public int _byteLenght;
    public void SetPacket(Packet packet, int byteLenght)
    {
        _packet = packet;
        _byteLenght = byteLenght;
    }
    public virtual void Handler()
    {
        /* OVERRIDE */
    }
}
