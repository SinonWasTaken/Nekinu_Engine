namespace Nekinu.Networking.Client
{
    class ClientSend
    {
        public static void WelcomeRecieved()
        {
            using (Packet packet = new Packet((int)ClientPackets.WelcomeRecieved))
            {
                packet.Write(Client.instance.myID);

                SendTCPData(packet);
            }
        }

        private static void SendTCPData(Packet _packet)
        {
            _packet.WriteLength();
            Client.instance.tcp.SendData(_packet);
        }
    }
}
