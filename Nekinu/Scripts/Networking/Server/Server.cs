using Nekinu.Editor;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Nekinu.Networking.Server
{
    public class Server : Component
    {
        public static Server Instance;

        public int ticks_per_second 
        {
            get => ticks;
            set
            {
                ticks = value;
                if(ticks != 0)
                    ms_per_tick = 100f / (float)ticks;
            }
        }

        private int ticks;

        public float ms_per_tick { get; private set; }

        public int MaxConnections { get; set; }
        public int Port { get; set; }

        private Dictionary<int, ServerClient> clients = new Dictionary<int, ServerClient>();

        public bool isOnline;

        private TcpListener tcpListener;

        public delegate void PacketHandler(int id, Packet packet);
        public Dictionary<int, PacketHandler> packetHandlers;

        public override void Awake()
        {
            base.Awake();

            StartServer();

            UpdateServer();
        }

        private void StartServer()
        {
            if (Instance != null && Instance != this)
                Instance.Destroy(parent);

            Instance = this;

            if(ticks_per_second > 0)
                ms_per_tick = 100 / ticks_per_second;

            if (MaxConnections <= 0)
            {
                Debug.WriteLine($"Error starting server! {MaxConnections} is too low. Must be 1 or above!");
                return;
            }

            if(Port <= 0)
            {
                Debug.WriteLine($"Error starting server! Port cannot be less than or equal to 0!");
                return;
            }

            InitalizeServerDictionary();

            Debug.WriteLine("Starting server!");

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallBack), null);

            isOnline = true;
        }

        public void Disconnect_Client(int ID)
        {
            clients[ID].Disconnect();
        }

        public EndPoint getClientEndPoint(int ID)
        {
            return clients[ID].tcp.socket.Client.RemoteEndPoint;
        }

        public bool isClientActive(int id)
        {
            return clients[id].tcp.socket != null ? true : false;
        }

        public void SendDataToClient(int ID, Packet packet)
        {
            clients[ID].tcp.SendData(packet);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            StopServer();
        }

        public void StopServer()
        {
            isOnline = false;
            tcpListener.Stop();
            tcpListener = null;

            Instance = null;

            Debug.WriteLine("Stopping server!");
        }

        private void TCPConnectCallBack(IAsyncResult ar)
        {
            if (isOnline)
            {
                TcpClient _client = tcpListener.EndAcceptTcpClient(ar);

                tcpListener.BeginAcceptTcpClient(TCPConnectCallBack, null);

                Debug.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}!");

                for (int i = 0; i < MaxConnections; i++)
                {
                    if (clients[i].tcp.socket == null)
                    {
                        clients[i].tcp.Connect(_client);
                        return;
                    }
                }

                Debug.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect! Server full!");
            }
        }

        private void InitalizeServerDictionary()
        {
            for (int i = 1; i < MaxConnections; i++)
            {
                clients.Add(i, new ServerClient(i));
            }

            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.WelcomeRecieved, ServerPacketHandle.WelcomeRecieved },
                { (int)ClientPackets.Disconnect, ServerPacketHandle.PlayerDisconnecting}
            };
        }

        public bool isServerEmpty()
        {
            for (int i = 0; i < MaxConnections; i++)
            {
                if (clients[i].tcp.socket != null)
                    return false;
            }

            return true;
        }

        public async Task<bool> UpdateServer()
        {
            DateTime now = DateTime.Now;

            while (isOnline)
            {
                while (!isServerEmpty())
                {
                    while (now < DateTime.Now)
                    {
                        ThreadManager.UpdateMain();

                        now = now.AddMilliseconds(ms_per_tick);

                        if (now > DateTime.Now)
                        {
                            await new WaitForSeconds((now.Millisecond - DateTime.Now.Millisecond) / 1000f).run();
                        }
                    }
                }
            }

            return true;
        }
    }
}
