using System;
using System.Net.Sockets;

namespace Nekinu.Networking.Server
{
    class ServerClient
    {
        public static int dataBufferSize = 4096;

        public int ID { get; private set; }

        public TCP tcp;

        public ServerClient(int _id)
        {
            ID = _id;
            tcp = new TCP(ID);
        }

        public void Disconnect()
        {
            tcp.Disconnect();
        }

        public class TCP
        {
            public TcpClient socket;

            private readonly int ID;
            private Packet recievedData;

            private NetworkStream stream;

            private byte[] recieveBuffer;

            public TCP(int _id)
            {
                ID = _id;
            }

            public void Connect(TcpClient _socket)
            {
                socket = _socket;

                socket.ReceiveBufferSize = dataBufferSize;

                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();

                recievedData = new Packet();
                recieveBuffer = new byte[dataBufferSize];

                stream.BeginRead(recieveBuffer, 0, dataBufferSize, new AsyncCallback(RecieveCallBack), null);

                ServerSendData.Welcome(ID, "Welcome to the server!");
            }

            public void Disconnect()
            {
                socket.Close();

                stream.Dispose();

                stream = null;

                recieveBuffer = null;
                recievedData = null;

                socket = null;
            }

            public void SendData(Packet packet)
            {
                try
                {
                    if(socket != null)
                    {
                        stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error sending data to connection {ID}: Error {e}");
                }
            }

            private void RecieveCallBack(IAsyncResult ar)
            {
                try
                {
                    int _byteLength = stream.EndRead(ar);

                    if(_byteLength <= 0)
                    {
                        Server.Instance.Disconnect_Client(ID);
                        return;
                    }

                    byte[] data = new byte[_byteLength];
                    Array.Copy(recieveBuffer, data, _byteLength);

                    recievedData.Reset(HandleData(data));
                    stream.BeginRead(recieveBuffer, 0, dataBufferSize, RecieveCallBack, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Client error on recieving data! {e}");
                    Server.Instance.Disconnect_Client(ID);
                }
            }

            private bool HandleData(byte[] data)
            {
                int packetLength = 0;

                recievedData.SetBytes(data);

                if(recievedData.UnreadLength() >= 4)
                {
                    packetLength = recievedData.ReadInt();

                    if(packetLength <= 0)
                    {
                        return true;
                    }
                }

                while(packetLength > 0 && packetLength <= recievedData.UnreadLength())
                {
                    byte[] packetBytes = recievedData.ReadBytes(packetLength);

                    ThreadManager.ExecuteOnMainThread(() => 
                    {
                        using (Packet _packet = new Packet(packetBytes))
                        {
                            int id = _packet.ReadInt();

                            Server.Instance.packetHandlers[id](ID, _packet);
                        }
                    });

                    packetLength = 0;
                    if(recievedData.UnreadLength() >= 4)
                    {
                        packetLength = recievedData.ReadInt();

                        if (packetLength <= 0)
                            return true;
                    }
                }
                if (packetLength <= 1)
                    return true;

                return false;
            }
        }
    }
}
