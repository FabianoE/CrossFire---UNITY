using AssetBundleManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public PlayerManager myPlayer(int id)
    {
        return players[id];
    }

    public void SpawnPlayer(int _id, int team,string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
            UIPVPManager.instance.SetImageScoreBoard(team);
        }
        else
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }

        _player.GetComponent<PlayerManager>().id = _id;
        _player.GetComponent<PlayerManager>().username = _username;
        _player.GetComponent<PlayerManager>().UISetHealth(100);
        _player.GetComponent<PlayerManager>().MyTeam = team;
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    public void SpawnpP(int _id, int team, string _username, Vector3 _position, Quaternion _rotation, Action ac)
    {
        SpawnP(_id, team, _username, _position, _rotation, ac);
    }

    public void SpawnP(int _id, int team, string _username, Vector3 _position, Quaternion _rotation, Action ac)
    {

        if (players.ContainsKey(_id))
            return;

        GameObject _player;
        if(localPlayerPrefab == null)
        {
            localPlayerPrefab = ABManager.instance.gameObjectList["Characters/LocalPlayer"];
        }

        if(playerPrefab == null)
        {
            playerPrefab = ABManager.instance.gameObjectList["Characters/LocalPlayer2"];
        }

        //GameObject sw = team == 0 ? localPlayerPrefab : playerPrefab;
        GameObject sw = localPlayerPrefab;

        if (_id == Client.instance.myId)
        {
            _player = Instantiate(sw, _position, _rotation);
            UIPVPManager.instance.gameObject.SetActive(true);
            UIPVPManager.instance.SetImageScoreBoard(team);
        }
        else
        {
            _player = Instantiate(sw, _position, _rotation);
        }


        _player.GetComponent<PlayerManager>().id = _id;
        _player.GetComponent<PlayerManager>().username = _username;
        _player.GetComponent<PlayerManager>().UISetHealth(100);
        _player.GetComponent<PlayerManager>().MyTeam = team;

        players.Add(_id, _player.GetComponent<PlayerManager>());
        ac.Invoke();
    }
}
