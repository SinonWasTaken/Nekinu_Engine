using System;
using System.Collections.Generic;
using System.Text;

namespace Nekinu.Networking.Server
{
    class ServerSendData
    {
        public static void Welcome(int _id, string msg)
        {
            using (Packet packet = new Packet((int)ServerPackets.Welcome))
            {
                packet.Write(msg);
                packet.Write(_id);

                SendTCPData(_id, packet);
            }
        }

        public static void ServerStopping()
        {
            using (Packet packet = new Packet((int)ServerPackets.Disconnect))
            {
                packet.Write("Stopping Server");
                SendTCPDataToAll(packet);
            }
        }

        private static void SendTCPDataToAll(Packet packet)
        {
            for (int i = 0; i < Server.Instance.MaxConnections; i++)
            {
                if (Server.Instance.isClientActive(i))
                {
                    Server.Instance.SendDataToClient(i, packet);
                }
            }
        }

        private static void SendTCPData(int id, Packet packet)
        {
            packet.WriteLength();

            Server.Instance.SendDataToClient(id, packet);
        }
    }
}
