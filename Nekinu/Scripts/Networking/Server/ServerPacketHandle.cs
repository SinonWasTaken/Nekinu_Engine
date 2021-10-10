using System;

namespace Nekinu.Networking.Server
{
    class ServerPacketHandle
    {
        public static void WelcomeRecieved(int id, Packet packet)
        {
            int clientID = packet.ReadInt();

            if(id != clientID)
            {
                Console.WriteLine($"{Server.Instance.getClientEndPoint(id)} has assumed the wrong ID!");
                Server.Instance.Disconnect_Client(id);
                return;
            }

            Console.WriteLine($"{Server.Instance.getClientEndPoint(id)} has connected!");
        }

        public static void PlayerDisconnecting(int id, Packet packet)
        {
            Console.WriteLine($"User {id} disconnecting. Disconnect reason {packet.ReadString()}");
        }
    }
}
