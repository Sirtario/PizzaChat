using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PIZZA.Chat.Server
{
    public class ChatClientConnection
    {

        public ChatClientConnection(string clientID, IPEndPoint endPoint)
        {
            ClientID = clientID;
            ClientIP = endPoint;
        }

        public ChatClientConnection(string clientID, IPEndPoint clientIP, string courentChannel)
        {
            ClientID = clientID;
            ClientIP = clientIP;
            CourentChannel = courentChannel;
        }

        public string ClientID { get; private set; }
        public IPEndPoint ClientIP { get; private set; }
        public string CourentChannel { get; set; }
        public byte Channelpower { get; set; }
    }
}
