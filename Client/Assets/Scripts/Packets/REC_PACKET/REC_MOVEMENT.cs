using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class REC_POS : PacketHandle
{
    public override void Handler()
    {
        int _id = _packet.ReadInt();
        Vector3 pos = _packet.ReadVector3();
        Quaternion rot = _packet.ReadQuaternion();

        if (!GameManager.players.ContainsKey(_id))
            return;

        if (_id != Client.instance.myId)
        {
            var player = GameManager.players[_id];
            player.position = pos;
            //player.transform.position = Vector3.Lerp(player.transform.position ,pos, 5f);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation ,rot, 1f);
        }
    }
}
public class REC_MOVEMENT : PacketHandle
{
    public override void Handler()
    {
        int _id = _packet.ReadInt();

        Vector3 pos = _packet.ReadVector3();
        GameManager.players[_id].transform.position = pos;
    }
}

public class REC_ROTATION : PacketHandle
{
    public override void Handler()
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = _rotation;
    }
}
