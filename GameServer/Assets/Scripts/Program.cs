using UnityEngine;
using System;
using System.Threading;


namespace GameServer
{
    class Program : MonoBehaviour
    {
        private static bool isRunning = false;

        void Start()
        {
            try
            {
                Console.Title = "Game Server";
                isRunning = true;

                Thread mainThread = new Thread(new ThreadStart(MainThread));
                 mainThread.Start();

                Thread secondThread = new Thread(new ThreadStart(SecondThread));
                secondThread.Start();

                Server.Start(50, 39191);
            }catch(Exception ex)
            {
                UnityEngine.Debug.LogWarning(ex.Message);
                Console.Read();
            }
        }

        private static void MainThread()
        {
            Debug.LogWarning($"[->] Main thread started. Running at {Constants.TICKS_PER_SEC} ticks per second.");
            DateTime _nextLoop = DateTime.Now;

            while (isRunning)
            {
                while (_nextLoop < DateTime.Now)
                {
                    GameLogic.Update();

                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK);

                    if (_nextLoop > DateTime.Now)
                    {
                        TimeSpan sp = _nextLoop - DateTime.Now;
                        if(sp.TotalSeconds >= 0)
                            Thread.Sleep(sp);
                    }
                }
            }
        }

        private static void SecondThread()
        {
            while (isRunning)
            {
                foreach(Room room in Game.RoomList.rooms.Values)
                {
                    if(room != null && room.users.Count > 0)
                    {
                        room.SendScore();
                    }
                    else if(room.users.Count <= 0)
                    {
                        Game.RoomList.RemoveRoom(room);
                        Game.RoomList.SendListAllPlayers();
                    }
                }

                Thread.Sleep(5000);
            }
        }
    }
}
