using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REC_DISCONNECTED : PacketHandle
{
    public override void Handler()
    {
        int _id = _packet.ReadInt();

        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

}
