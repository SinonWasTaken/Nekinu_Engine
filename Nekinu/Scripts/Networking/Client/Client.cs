using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Nekinu.Networking.Client
{
    public class Client : Component
    {
        public static Client instance;

        public static int dataBufferSize = 4096;

        public string ipAddress = "127.0.0.1";
        public int port = 26950;
        public int myID = 0;
        public TCP tcp;

        public bool isConnected = false;

        private delegate void PacketHandler(Packet _packet);
        private static Dictionary<int, PacketHandler> packetHandlers;

        public override void Awake()
        {
            base.Awake();
            if (instance != null && instance != this)
            {
                Destroy(instance.parent);
            }

            instance = this;
        }

        public override void Start()
        {
            base.Start();

            tcp = new TCP();

            UpdateThread();
        }


        public override void Update()
        {
            base.Update();
            
            if(!isConnected)
                ConnectToServer();
        }

        private async Task<bool> UpdateThread()
        {
            DateTime now = DateTime.Now;

            Entity serverentity = SceneManage.SceneManager.loadedScene.GetEntity("Server");

            Server.Server server = serverentity.GetComponent<Server.Server>();

            if (serverentity != null)
            {
                while (isConnected)
                {
                    while (now < DateTime.Now)
                    {
                        ThreadManager.UpdateMain();

                        now = now.AddMilliseconds(server.ms_per_tick);

                        if (now > DateTime.Now)
                        {
                            await new WaitForSeconds((now.Millisecond - DateTime.Now.Millisecond) / 1000f).run();
                        }
                    }
                }
            }

            return true;
        }

        public void ConnectToServer()
        {
            InitializeClientData();
            tcp.TCPConnect();
        }

        public void ConnectToServer(string ip)
        {
            InitializeClientData();
            ipAddress = ip;
            tcp.TCPConnect();
        }

        public void DisconnectFromServer()
        {
            if (isConnected)
            {
                isConnected = false;

                tcp.socket.Close();
            }
        }

        public class TCP
        {
            public TcpClient socket;

            private NetworkStream stream;

            private Packet recievedData;
            private byte[] recieveBuffer;

            public void TCPConnect()
            {
                socket = new TcpClient()
                {
                    ReceiveBufferSize = dataBufferSize,
                    SendBufferSize = dataBufferSize
                };

                recieveBuffer = new byte[dataBufferSize];

                socket.BeginConnect(instance.ipAddress, instance.port, ConnectCallBack, socket);
            }

            private void ConnectCallBack(IAsyncResult ar)
            {
                socket.EndConnect(ar);

                if (!socket.Connected)
                {
                    return;
                }

                instance.isConnected = true;

                stream = socket.GetStream();

                recievedData = new Packet();

                stream.BeginRead(recieveBuffer, 0, dataBufferSize, RecieveCallBack, null);
            }

            public void Disconnect()
            {
                instance.DisconnectFromServer();
                stream = null;

                recievedData.Dispose();
                recieveBuffer = null;

                socket = null;
            }

            private void RecieveCallBack(IAsyncResult ar)
            {
                try
                {
                    int _byteLength = stream.EndRead(ar);
                    if (_byteLength <= 0)
                    {
                        instance.DisconnectFromServer();
                        return;
                    }

                    byte[] _data = new byte[_byteLength];
                    Array.Copy(recieveBuffer, _data, _byteLength);
                    recievedData.Reset(HandleData(_data));
                    stream.BeginRead(recieveBuffer, 0, dataBufferSize, RecieveCallBack, null);
                }
                catch (Exception e)
                {
                    Disconnect();
                }
            }

            private bool HandleData(byte[] data)
            {
                int _packetLength = 0;

                recievedData.SetBytes(data);

                if (recievedData.UnreadLength() >= 4)
                {
                    _packetLength = recievedData.ReadInt();

                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }

                while (_packetLength > 0 && _packetLength <= recievedData.UnreadLength())
                {
                    byte[] _packetBytes = recievedData.ReadBytes(_packetLength);
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet _packet = new Packet(_packetBytes))
                        {
                            int id = _packet.ReadInt();
                            packetHandlers[id](_packet);
                        }
                    });

                    _packetLength = 0;
                    if (recievedData.UnreadLength() >= 4)
                    {
                        _packetLength = recievedData.ReadInt();

                        if (_packetLength <= 0)
                        {
                            return true;
                        }
                    }
                }

                if (_packetLength <= 1)
                {
                    return true;
                }

                return false;
            }

            public void SendData(Packet packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error sending data {e}");
                }
            }
        }

        private void InitializeClientData()
        {
            packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ServerPackets.Welcome, ClientHandle.Welcome},
            { (int)ServerPackets.Disconnect, ClientHandle.Disconnect}
        };
        }
    }
}
