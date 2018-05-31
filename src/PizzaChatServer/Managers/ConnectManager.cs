using System;
using System.Net;
using PIZZA.Chat.Core;

namespace PIZZA.Chat.Server
{
    internal class ConnectManager
    {
        public event ChatClientConnectionApprovalEventHandler ClientConnectionApprove;
        public event Action<PizzaChatMessage, IPEndPoint> SendMessage;
        internal event Action<ChatClientConnection> ClientConnected;

        internal void RecieveConnect(PizzaChatMessage message, ChatClientConnection connection)
        {
            var Fixedheader = message.FixedHeader;
            var Varheader = message.VariableHeader as ChatVarHeaderConnect;

            var eventargs = new ChatConnectApprovalEventArgs(Varheader.ClientID, Varheader.Password);

            ClientConnectionApprove.Invoke(this, eventargs);

            SendMessage.Invoke(GenerateAckMessage(eventargs.ConnectReturncode, eventargs.CommunicationMode, eventargs.PingIntervall), connection.ClientIP);

            if (eventargs.ConnectReturncode == ChatConnectReturncode.ACCEPTED)
            {
                ClientConnected.Invoke(new ChatClientConnection(Varheader.ClientID, connection.ClientIP, eventargs.PingIntervall));
            }
        }

        private PizzaChatMessage GenerateAckMessage(ChatConnectReturncode connectReturncode, byte comMode, byte pingintervall)
        {
            var ackmessage = new PizzaChatMessage(Packettypes.CONNACK);

            var varheader = ackmessage.VariableHeader as ChatVarHeaderConnAck;
            varheader.CommunicationMode = comMode;
            varheader.PingIntervall = pingintervall;
            varheader.Returncode = connectReturncode;

            ackmessage.FixedHeader.RemainingLength = varheader.Lenght.Value;

            return ackmessage;
        }
    }
}