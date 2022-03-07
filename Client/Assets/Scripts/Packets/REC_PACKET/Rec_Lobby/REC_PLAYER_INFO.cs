using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class REC_PLAYER_INFO : MonoBehaviour
{
    public static void Received(Packet packet)
    {
        int player_cash = packet.ReadInt();
        int player_gold = packet.ReadInt();
        int player_exp = packet.ReadInt();
        int player_kills = packet.ReadInt();
        int player_deaths = packet.ReadInt();
        string player_name = packet.ReadString();

        LobbyItemsObject.instance.SetPlayerInfo(player_name, player_cash, player_gold, player_exp, player_kills, player_deaths);
    }
}

