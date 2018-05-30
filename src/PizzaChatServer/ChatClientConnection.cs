using System;
using System.Collections.Generic;
using System.Net;
using System.Timers;

namespace PIZZA.Chat.Server
{
    public class ChatClientConnection
    {

        public ChatClientConnection(string clientID, IPEndPoint endPoint, int PingInterval)
        {
            ClientID = clientID;
            ClientIP = endPoint;
            Pinginterval = PingInterval;

            PingTimer = new Timer((Pinginterval / 1000) + 2);//umrechnung von sek. in Millisek.
            PingTimer.Start();
        }

        public ChatClientConnection(string clientID, IPEndPoint clientIP,int PingInterval, string courentChannel)
        {
            ClientID = clientID;
            ClientIP = clientIP;
            Pinginterval = PingInterval;
            CourentChannel = courentChannel;

            PingTimer = new Timer((Pinginterval / 1000) + 2);//umrechnung von sek. in Millisek.
            PingTimer.Start();
        }

        public string ClientID { get; private set; }
        public IPEndPoint ClientIP { get; private set; }
        public int Pinginterval { get; private set; }
        public string CourentChannel { get; set; }
        public byte Channelpower { get; set; }

        public Timer PingTimer { get; private set; }
    }
}
