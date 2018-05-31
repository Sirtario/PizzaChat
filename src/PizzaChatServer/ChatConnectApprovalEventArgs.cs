using PIZZA.Chat.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Chat.Server
{
    public delegate void ChatClientConnectionApprovalEventHandler(object sender, ChatConnectApprovalEventArgs e); 

    /// <summary>
    /// Holds everything to check if a client is whitelisted via ACL files
    /// </summary>
    public class ChatConnectApprovalEventArgs
    {
        public ChatConnectApprovalEventArgs(string clientID, string password)
        {
            ClientID = clientID;
            Password = password;
        }

        public string ClientID { get; set; }
        public string Password { get; set; }
        public ChatConnectReturncode ConnectReturncode { get; set; }
        public byte CommunicationMode { get; set; }
        /// <summary>
        /// Pingintervall in Sek.
        /// </summary>
        public byte PingIntervall { get; set; }
    }
}
