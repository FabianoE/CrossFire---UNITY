using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GameServer.RoomModes;

namespace GameServer
{
    public class Room : IDisposable
    {
        ~Room()
        {
            GC.Collect();
        }

        public Thread updateThread;

        public bool started = false;

        public Room()
        {
            Start = DateTime.Now;
            //
            updateThread = new Thread(Update);
            updateThread.Start();
            CreateSlots();
            SetRoomMode();
        }

        //Room Modes(TDM, FFA)
        public TDM TDM = null;
        public FFA FFA = null;

        /////////////////////////////////////
        //Basic Info Room
        public int id;
        public int hostid;
        public int maxPlayer;
        public string name;
        public Enums.RoomStatusEnum roomStatus = Enums.RoomStatusEnum.NORMAL;

        public Slot[] slots = new Slot[16];

        //GameMode Info
        public Enums.RoomMode mode = Enums.RoomMode.TDM;
        public int map;
        public Enums.RoomTypes type; //Modo da partida, KILL OU TEMPO
        public int typeoption; //Objetivo do tipo
        public int respawnTime;
        public int kills = 0;

        public int ffakills = 0;
        public int blkills = 0;
        public int grkills = 0;

        public int RoomObjective; //Room Max Kills or Time;
        public int spawnLocaltion = -1;

        //RoomTime
        public int Lastick = 0;
        public DateTime Start;

        public ConcurrentDictionary<int, Client> users = new ConcurrentDictionary<int, Client>();

        public void AddPlayer(Client player)
        {
            users.TryAdd(player.id, player);

            player.player.room = this;

            int playerslot = AddPlayerInSloot(player.player);

            SendData(new Packets.SERVER.Lobby.InRoom.SEND_ENTER_ROOM(player.player, playerslot));
            SendData(new Packets.SERVER.Lobby.InRoom.SEND_CHAT(null, string.Concat("O jogador ", player.player.username, " acabou de entrar")));
        }

        public void StartUDP(Client player)
        {
            SendData(new SERVER.SEND_ROOM_DATA(this));
            player.tcp.SendData(new SERVER.SEND_ROOM_DATA(player));

            SendScore();
        }


        public void StartBattle()
        {
            UnityEngine.Debug.LogWarning("START BATTLE");
            Start = DateTime.Now;
            started = true;
            roomStatus = Enums.RoomStatusEnum.BATTLE;

            int verifyStart = 0;
            for(int i = 0; i < slots.Length; i++)
            {
                if(slots[i].player != null && slots[i].slotStatus == Enums.SlotStatusEnum.BATTLE_READY)
                {
                    verifyStart++;
                }
            }

            if (verifyStart <= 1 && !Constants.Test)
            {
                StopBattle();
                return;
            }

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].player != null && slots[i].slotStatus == Enums.SlotStatusEnum.BATTLE_READY)
                {
                    new Packets.SERVER.SEND_SPAWN(slots[i].player.id);

                    slots[i].player.KillInRoom = 0;
                    slots[i].player.DeathInRoom = 0;
                    SendData(new SERVER.SEND_ROOM_DATA(this));

                    Server.clients[slots[i].player.id].tcp.SendData(new SERVER.SEND_ROOM_DATA(Server.clients[slots[i].player.id]));

                    SendScore();
                }
            }
        }

        public void StopBattle()
        {
            UnityEngine.Debug.LogWarning("STOP BATTLE");

            SendData(new Packets.SERVER.Lobby.Battle.SEND_STOP_BATTLE(this));

            kills = 0;
            blkills = 0;
            grkills = 0;
            ffakills = 0;

            started = false;
        }
        /// <summary>
        /// Set TDM, FFA and others
        /// </summary>
        public void SetRoomMode()
        {
            switch (mode)
            {
                case Enums.RoomMode.TDM:
                    TDM = new TDM(this);
                    break;
                case Enums.RoomMode.FFA:
                    FFA = new FFA(this);
                    break;
            }
        }

        void CreateSlots()
        {
            for (int i = 0; i < 16; i++)
            {
                slots[i] = new Slot(i);
            }
        }

        public int AddPlayerInSloot(Player player)
        {
            for (int i = 0; i < 16; i++)
            {
                Slot slot = slots[i];
                if (slot != null && slot.player == null)
                {
                    slot.player = player;
                    slot.exp = 0;
                    slot.gold = 0;
                    slot.slotStatus = Enums.SlotStatusEnum.NORMAL;
                    player.slotId = slot.id;
                    player.Team = slot.id % 2;

                    UnityEngine.Debug.LogWarning($"Player {player.username} added in team: {player.Team}");
                    return i;
                }
            }

            return -1;
        }

        public int GetSlotNullByTeam(int team)
        {
            for (int i = 0; i < 16; i++)
            {
                if ((i % 2) != team || slots[i].player != null)
                    continue;

                return i;
            }

            return -1;
        }

        public int GetCountPlayersByTeam(int team)
        {
            int t = 0;
            foreach (var user in users.Values)
            {
                if (user.player.Team == team)
                    t++;
            }
            return t;
        }

        public int GetReadyPlayersByTeam(int team)
        {
            int t = 0;
            foreach(var slot in slots)
            {
                if (slot.id % 2 == team)
                    if (slot.slotStatus == Enums.SlotStatusEnum.READY)
                        t++;
            }

            return t;
        }

        public bool EnableToStart() //INCOMPLETE
        {
            int team1 = GetReadyPlayersByTeam(0);
            int team2 = GetReadyPlayersByTeam(1);
            //if (Math.Abs(team1 - team2))
            return true;
        } 

        public Slot GetSlotPlayer(Player player)
        {
            return slots.First(x => x.player == player);
        }

        public Player GetAcePlayer()
        {
            Player player = null;
            foreach (var obj in users.Values.OrderByDescending(x => x.player.KillInRoom).OrderBy(x => x.player.DeathInRoom))
            {
                player = obj.player;
                break;
            }

            return player;

        }

        public void SendData(Packet packet)
        {
            try
            {
                foreach (Client client in users.Values)
                {
                    try
                    {
                        if (client != null)
                            client.tcp.SendData(packet);
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning("Room Error: " + ex.Message);
            }
        }

        public void SendToPlayerInBattle(Packet packet)
        {
            try
            {
                foreach (Client player in users.Values)
                    if (player != null)
                        if (slots[player.player.slotId].player != null)
                            if (slots[player.player.slotId].slotStatus == Enums.SlotStatusEnum.BATTLE_READY)
                                player.tcp.SendData(packet);
            }
            catch (Exception ex) { UnityEngine.Debug.LogWarning("BATTLE:: " + ex.Message); }
        }

        public void SendScore()
        {
            List<Player> playerList = new List<Player>();
            foreach (var u in slots)
            {
                if (u.slotStatus == Enums.SlotStatusEnum.BATTLE_READY)
                    playerList.Add(u.player);
            }

            SendToPlayerInBattle(new SERVER.SEND_ROOM_DATA(playerList, this));
        }

        public void Disconnected(Client client)
        {
            Slot slot = slots.First(x => x.player == client.player);
            SendData(new Packets.SERVER.Lobby.InRoom.SEND_LEAVE_ROOM(slot.id, slot.player.id));
            slot.Clear();

            client.player.room = null;

            users.TryRemove(client.id, out client);
            UnityEngine.Debug.LogWarning("Player saiu");

            SendScore();

            if (mode != Enums.RoomMode.FFA)
            {
                int lenghtUsersBL = 0;
                int lenghtUsersGR = 0;

                for (int i = 0; i < slots.Length; i++)
                    if (slots[i].slotStatus == Enums.SlotStatusEnum.BATTLE_READY && slots[i].id % 2 == 1)
                        lenghtUsersBL++;

                for (int i = 0; i < slots.Length; i++)
                    if (slots[i].slotStatus == Enums.SlotStatusEnum.BATTLE_READY && slots[i].id % 2 == 0)
                        lenghtUsersGR++;

                if (lenghtUsersBL == 0 || lenghtUsersGR == 0)
                    StopBattle();
            }
            else
            {
                int lenghtFFA = 0;
                for (int i = 0; i < slots.Length; i++)
                    if (slots[i].slotStatus == Enums.SlotStatusEnum.BATTLE_READY)
                        lenghtFFA++;

                if (lenghtFFA <= 1)
                    StopBattle();
            }
        }

        void Update()
        {
            while (true)
            {
                if (started)
                {
                    Lastick = DateTime.Now.Second;

                    switch (mode)
                    {
                        case Enums.RoomMode.TDM:
                            TDM.Update();
                            break;
                        case Enums.RoomMode.FFA:
                            FFA.Update();
                            break;
                    }

                    SendData(new SERVER.SEND_ROOM_DATA(this));
                }
                Thread.Sleep(1000);
            }
        }

        public int GetTimeLeft()
        {
            if(Constants.Test)
                return 50 * 60 - (int)(DateTime.Now - Start).TotalSeconds;

            if (type == Enums.RoomTypes.Time)
                return RoomObjective * 60 - (int)(DateTime.Now - Start).TotalSeconds;

            return 2 * 60 - (int)(DateTime.Now - Start).TotalSeconds;
        }

        public List<Slot> GetSlotLoaded()
        {
            List<Slot> slota = new List<Slot>();
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].player != null && slots[i].slotStatus == Enums.SlotStatusEnum.BATTLE_READY)
                    slota.Add(slots[i]);
            }

            return slota;
        }

        public void Dispose()
        {

        }
    }
}
