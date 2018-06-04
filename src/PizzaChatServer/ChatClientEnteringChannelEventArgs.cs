using PIZZA.Chat.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Chat.Server
{
    public delegate void ChatClientEnteringChannelEventHandler(object sender, ChatClientEnteringChannelEventArgs e);

    public class ChatClientEnteringChannelEventArgs
    {
        public ChatClientEnteringChannelEventArgs(string clientID, string password,string channel, ChatEnterChannelReturnCode returnCode)
        {
            ClientID = clientID;
            Password = password;
            Channel = channel;
            ReturnCode = returnCode;
        }

        public string ClientID { get; private set; }
        public string Password { get; private set; }
        public string Channel { get; private set; }
        public ChatEnterChannelReturnCode ReturnCode { get; set; }
    }
}
