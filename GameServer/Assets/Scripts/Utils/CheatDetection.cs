using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
      class CheatDetection
      {
        public static bool CheckSpeed(Player player, DateTime Lasttime)
        {
            if (player.LastPingReceive == null)
            {
                player.LastPingReceive = Lasttime;
                return false;
            }
            else
            {
                TimeSpan span = Lasttime - player.LastPingReceive;
                if(span.TotalSeconds >= 5)
                {
                    //UnityEngine.Debug.LogWarning("SAFE");
                    player.LastPingReceive = Lasttime;
                    return false;
                }
                else
                {
                    //UnityEngine.Debug.LogWarning("HACKER");
                    player.LastPingReceive = Lasttime;
                    return false;
                }
            }
        }

      }
}
