using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace GameServer
{
    class Server : MonoBehaviour
    {
        public static GetData connectiondb = new GetData();
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public delegate void PacketHandler(int _fromClient, Packet _packet);
        public static Dictionary<int, PacketHandler> packetHandlers;

        public static Dictionary<int, Structure.PacketHandle> packetHandler;

        private static TcpListener tcpListener;
        private static UdpClient udpListener;

        public static void Start(int _maxPlayers, int _port)
        {
            try
            {
                MaxPlayers = _maxPlayers;
                Port = _port;

                Debug.LogWarning("[->] Starting server... \n[->] Checking connection with DB");

                if (!connectiondb.CheckConnection())
                    return;

                InitializeServerData();

                //tcpListener = new TcpListener(IPAddress.Parse("10.0.0.4"), Port);
                tcpListener = new TcpListener(IPAddress.Any, Port);
                tcpListener.Start();
                tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

                udpListener = new UdpClient(Port);
                //udpListener.Client.NoDelay = true;
                udpListener.BeginReceive(UDPReceiveCallback, null);

                udpListener.Client.IOControl(
                    (IOControlCode)Constants.SIO_UDP_CONNRESET,
                    new byte[] { 0, 0, 0, 0 },
                    null
                );

                //Game.RoomList.Setup();
                Debug.LogWarning($"[->] Server started on port {Port}, ip {IPAddress.Any}.");
                Debug.LogWarning("=====================================");

            }
            catch (Exception ex)
            {
                Debug.LogWarning("[->] ERROR: " + ex.Message);
            }
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
            Debug.LogWarning($"Incoming connection from {_client.Client.RemoteEndPoint}...");

            for (int i = 1; i <= MaxPlayers; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(_client);
                    return;
                }
            }

            Debug.LogWarning($"{_client.Client.RemoteEndPoint} failed to connect: Server full!");
        }

        private static void UDPReceiveCallback(IAsyncResult _result)
        {
            try
            {
                IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);

                udpListener.BeginReceive(UDPReceiveCallback, null);

                if (_data.Length < 4)
                {
                    return;
                }

                using (Packet _packet = new Packet(_data))
                {
                    int _clientId = _packet.ReadInt();

                    if (_clientId == 0)
                    {
                        return;
                    }

                    if (clients[_clientId].udp.endPoint == null)
                    {
                        clients[_clientId].udp.Connect(_clientEndPoint);
                        return;
                    }

                    if (clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                    {
                        clients[_clientId].udp.HandleData(_packet);
                    }
                }
            }
            catch (Exception _ex)
            {
                Debug.LogWarning($"Error receiving UDP data: {_ex}");
            }
        }

        public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
        {
            try
            {
                if (_clientEndPoint != null)
                {
                    udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.LogWarning($"Error sending data to {_clientEndPoint} via UDP: {_ex}");
            }
        }

        private static void InitializeServerData()
        {
            for (int i = 1; i <= MaxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }

            packetHandler = new Dictionary<int, Structure.PacketHandle>()
            {
                //Auth
                {(int)ClientPackets.authData, new Packets.CLIENT.Auth.REC_AUTH() },

                //Lobby
                {(int)ClientPackets.lobbyData, new Packets.CLIENT.Lobby.REC_LOBBYDATA() },

                //
                {(int)ClientPackets.welcomeReceived, new Packets.REC_WELCOME() },
                {(int)ClientPackets.playerMovement, new Packets.CLIENT.REC_MOVEMENT() },
                {(int)6, new Packets.CLIENT.REC_ANIM() },
                {(int)ClientPackets.playerChangeWeapon, new Packets.CLIENT.REC_CHANGE_WEAPON() },
                {(int)ClientPackets.playerShoot, new Packets.CLIENT.REC_SHOOT() },
                {(int)ClientPackets.playerDamage, new Packets.CLIENT.REC_DAMAGE() },
                {(int)ClientPackets.playerRespawn, new Packets.CLIENT.REC_RESPAWN() },
                {(int)ClientPackets.networkData, new Packets.CLIENT.REC_NETWORKPACKET() },
            };

            Debug.LogWarning("[->] Initialized packets.");
        }

        public void ReceivePacket()
        {

        }
    }
}
