using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    class Constants
    {
        public const int TICKS_PER_SEC = 30;
        public const float MS_PER_TICK = 1000f / TICKS_PER_SEC;
        public const float DelayReceiveMovement = 0.1f;
        public const bool Test = true;
        public const bool AutoCreateAccount = true;
        public const int MinPlayer_RoomStart = 1;
        //
        public const int SIO_UDP_CONNRESET = -1744830452;
    }
}
