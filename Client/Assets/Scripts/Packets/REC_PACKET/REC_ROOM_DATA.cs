using AssetBundleManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REC_ROOM_DATA : PacketHandle
{
    public override void Handler()
    {
        int packetid = _packet.ReadInt();

        switch (packetid)
        {
            case 1: //Receive Time
                try {

                    int roomTime = _packet.ReadInt();
                    string time = $"{TimeSpan.FromSeconds(roomTime).Minutes}:{TimeSpan.FromSeconds(roomTime).Seconds}";
                    UIPVPManager.instance.SetScoreBoard(time);

                } catch (Exception ex) { Debug.LogError(ex.Message); }
                break;

            case 2: //Receive Kill
                try {
                    int objective = _packet.ReadInt();

                    int myteam = _packet.ReadInt();
                    int otherteam = _packet.ReadInt();

                    PlayerManager myplayer = GameManager.instance.myPlayer(Client.instance.myId);

                    var player = GameManager.players[_packet.ReadInt()].GetComponent<PlayerManager>();
                    var target = GameManager.players[_packet.ReadInt()].GetComponent<PlayerManager>();

                    int weaponid = _packet.ReadInt();
                    int type = _packet.ReadInt();
                    int numkills = _packet.ReadInt();

                    int mteam = myteam;
                    int oteam = otherteam;

                    if(myplayer.MyTeam == 3)
                    {
                        mteam = PlayerManager.instance.Kills; ;
                       // oteam = oteam;
                    }
                    else if (player.MyTeam == myplayer.MyTeam) 
                    {
                        oteam = otherteam;
                        mteam = myteam;
                    }
                    else
                    {
                        mteam = otherteam;
                        oteam = myteam;
                    }

                    UIPVPManager.instance.SetScoreBoard(mteam, oteam, objective);
                    UIPVPManager.instance.SpawnKillFeed(player, target, weaponid, type, numkills);

                } catch (Exception ex) { Debug.LogError(ex.Message); }
                break;

            case 3: //New Player(Me/Eu)
                try
                {

                    //Instantiate<GameObject>(ABManager.instance.gameObjectList["UI/PVP UI"]);
                    int objective = _packet.ReadInt();
                    int roomMode = _packet.ReadInt();
                    int mteam = _packet.ReadInt();
                    int myteam = _packet.ReadInt();
                    int otherteam = _packet.ReadInt();

                    UIPVPManager.instance.SetScoreBoard(myteam, otherteam, objective);
                    UIScoreBoard.instance.DefineUI(mteam);

                    if (roomMode == 1) // == FFA
                        mteam = 3;

                    Client.instance.myTeam = mteam;
                }
                catch (Exception ex) { Debug.LogError(ex.Message); }
                break;

            case 4: //Score Data
                try
                {
                    int roomMode = _packet.ReadInt();
                    int lenght = _packet.ReadInt();
                    byte[] bt = _packet.ReadBytes(lenght);

                    Player[] pl = bt.DeserializeObject<Player[]>();

                    List<Player> pll = new List<Player>();
                    foreach (Player p in pl)
                    {
                        pll.Add(p);
                    }

                    UIScoreBoard.instance.SetUI(pll, roomMode);

                }
                catch(Exception ex) { Debug.LogError(ex.Message); }
                break;
        }
    }
}
