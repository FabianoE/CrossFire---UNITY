using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using GameServer;
using System.Net.NetworkInformation;
using System.Threading;
using System.Net;
using GameServer.Enums;

[Serializable]
public class Player
{
    [NonSerialized] public Client client = null;

    public int id;
    public int dbid;
    public string username;
    public int gold, cash, exp, kills, deaths;
    public bool Alive = true;
    public bool inRoom = false;
    public int slotId = -1;

    public int weapon = 1;
    public List<InventoryItems> inventoryItems = new List<InventoryItems>();
    public List<InventoryItems> characterItems = new List<InventoryItems>();
    [NonSerialized] public string primaryweaponid = "1";
    [NonSerialized] public string secondaryweapon = "2";
    [NonSerialized] public string tercweapon = "4";
    [NonSerialized] public string grenadeweapon = "5"; //5 = grenade ID default 

    /// ////////////////////////////////////////////////////////////////////////////////////
    [NonSerialized] public RoomInventory roomInventory = new RoomInventory();
    /// ////////////////////////////////////////////////////////////////////////////////////

    public int Health = 100;
    public int Team = (int)Teams.None;

    //Kills Info
    public int KillInRoom = 0;
    public int DeathInRoom = 0;
    public int LastKills = 0;
    [NonSerialized] public DateTime LastKillTime = DateTime.Now;
    //
    public string ping;
    [NonSerialized] public string IP;

    //CheckInfo - AntiCheat
    [NonSerialized] public bool isReloading = false;
    [NonSerialized] public DateTime LastPingReceive;

    [NonSerialized] public Room room = null;
    [NonSerialized] public Vector3 position;
    [NonSerialized] public Quaternion rotation;

    [NonSerialized] public int ChangeWeaponCheck = 0;


    public Player(int _id, string[] data = null)
    {
        //player_name, player_cash, player_gold
        id = _id;
        dbid = int.Parse(data[0]);
        username = data[1];
        cash = int.Parse(data[2]);
        gold = int.Parse(data[3]);
        exp = int.Parse(data[4]);
        kills = int.Parse(data[5]);
        deaths = int.Parse(data[6]);

        client = Server.clients[_id];
        IPEndPoint remote = client.tcp.socket.Client.RemoteEndPoint as IPEndPoint;
        this.IP = remote.Address.ToString();

        Initialize();
    }

    void Initialize()
    {
        inventoryItems.AddRange(Server.connectiondb.GetInventoryItems(dbid));
        characterItems.AddRange(Server.connectiondb.GetInventoryItems(dbid, "player_characters"));

        rotation = Quaternion.Identity;
    }

    public void SetPosition(Vector3 v, Quaternion q)
    {
        if (position != v || q != rotation)
        {
            position = v;
            rotation = q;
        }
    }

    public void Kill()
    {
        if (room.mode != RoomMode.FFA)
        {
            if (Team == 0)
                room.blkills++;
            else
                room.grkills++;
        }
        else
        {
            room.ffakills++;
        }
        TimeSpan compareTime = DateTime.Now - LastKillTime;

        if (compareTime.TotalSeconds > 5)
        {
            LastKillTime = DateTime.Now;
            LastKills = 1;
        }
        else
        {
            LastKillTime = DateTime.Now;
            LastKills++;
        }
    }

    public void Update()
    {
        //RetrievePing();
    }

    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        rotation = _rotation;
    }

    public WeaponInBattleInventory getWeaponEquiped()
    {
        return roomInventory.GetEquipedWeapon(weapon);
    }

    public void AddWeapon(WeaponInBattleInventory item)
    {
        roomInventory.EquipWeapon(item);
    }

    public void AddWeapon(int ID)
    {
        roomInventory.EquipWeapon(ServerObjectsManager.instance.ST_WeaponsData.GetWeaponById(ID));
    }

    #region PingRegion

    public void RetrievePing()
    {
        try
        {
            Ping ping = new Ping();
            ping.PingCompleted += new System.Net.NetworkInformation.PingCompletedEventHandler(RetrievePing_Complete);
            ping.SendAsync(this.IP, 800);
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogWarning(ex.Message);
        }
    }

    private void RetrievePing_Complete(object sender, PingCompletedEventArgs e)
    {
        PingReply p = e.Reply;
        if (p.Status == IPStatus.Success)
        {
            if (p.RoundtripTime.ToString() != ping)
            {
                ping = p.RoundtripTime.ToString();
                UnityEngine.Debug.LogWarning(ping);
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning(p.Status);
        }
    }

    #endregion
}
