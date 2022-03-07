using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsMine : MonoBehaviour
{
    protected PlayerManager _client;

    public void Awake()
    {
        if (TryGetComponent<PlayerManager>(out PlayerManager player))
        {
            _client = player;
        }
        else
            _client = transform.root.GetComponent<PlayerManager>();
    }

    public bool isMine(Client client = null)
    {
        return _client.id == Client.instance.myId;
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
