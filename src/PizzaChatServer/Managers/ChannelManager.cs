using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using PIZZA.Chat.Core;

namespace PIZZA.Chat.Server
{
    internal class ChannelManager
    {
        public event ChatClientEnteringChannelEventHandler ClientEnteringChannel;

        public event Action<PizzaChatMessage, IPEndPoint> SendMessage;

        internal void RecieveEnterChannel(PizzaChatMessage message, ChatClientConnection connection, List<PIZZAChannel> channels)
        {
            var varheader = message.VariableHeader as ChatVarHeaderEnterChannel;
            var eventargs = new ChatClientEnteringChannelEventArgs(connection.ClientID,varheader.Password, ChatEnterChannelReturnCode.Denied);

            if (!channels.Any(c => c.Channelname.Value == varheader.Channel))
            {
                eventargs.ReturnCode = ChatEnterChannelReturnCode.DeniedChannelNotAvailable;
            }
            else
            {
                ClientEnteringChannel.Invoke(this, eventargs);
            }

            SendMessage.Invoke( GenerateEnterChannelAckMessage(eventargs.ReturnCode), connection.ClientIP);
        }

        private PizzaChatMessage GenerateEnterChannelAckMessage(ChatEnterChannelReturnCode returnCode)
        {
            var message = new PizzaChatMessage(Packettypes.ENTERCHANNELACK);

            var varheader = message.VariableHeader as ChatVarHeaderEnterChannelAck;

            varheader.ReturnCode = returnCode;

            return message;
        }
    }
}