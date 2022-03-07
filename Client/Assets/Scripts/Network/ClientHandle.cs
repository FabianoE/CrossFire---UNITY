using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        //GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.players[_id].transform.position = _position;
    }

    public static void PlayerPosInput(Packet _packet)
    {
        int id = _packet.ReadInt();
        int l = _packet.ReadInt();

        bool i1 = _packet.ReadBool();
        bool i2 = _packet.ReadBool();
        bool i3 = _packet.ReadBool();
        bool i4 = _packet.ReadBool();

//        PlayerManager player = GameManager.players[id];

        if (i1)
            GameManager.players[id].position.x += 1;
        if(i2)
            GameManager.players[id].position.x -= 1;
        if(i3)
            GameManager.players[id].position.z += 1;
        if(i4)
            GameManager.players[id].position.z -= 1;



        Vector3 move = (Vector3.forward * 1) + (Vector3.right * 1);

        GameManager.players[id].transform.position = GameManager.players[id].position;
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = _rotation;
    }

    public static void PlayerMove(Packet _packet)
    {
        int _id = _packet.ReadInt();

        Vector3 pos = _packet.ReadVector3();
        GameManager.players[_id].transform.position = pos;

        Debug.LogWarning("POS "  + pos);
    }
}
