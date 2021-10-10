using System;

namespace Nekinu.Networking.Client
{
    class ClientHandle
    {
        public static void Welcome(Packet _packet)
        {
            string msg = _packet.ReadString();
            int my_ID = _packet.ReadInt();

            Client.instance.myID = my_ID;

            Console.WriteLine($"Message recieve from server: {msg}");

            ClientSend.WelcomeRecieved();
        }

        public static void Disconnect(Packet packet)
        {
            string msg = packet.ReadString();
            Console.WriteLine($"Disconnect from server! Disconnect reason {msg}");

            Client.instance.DisconnectFromServer();
        }
    }
}
