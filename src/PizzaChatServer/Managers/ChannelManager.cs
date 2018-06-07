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
        public event Action<string, string> ChannelChanged;

        internal void RecieveEnterChannel(PizzaChatMessage message, ChatClientConnection connection, List<PIZZAChannel> channels)
        {
            var varheader = message.VariableHeader as ChatVarHeaderEnterChannel;
            var eventargs = new ChatClientEnteringChannelEventArgs(connection.ClientID,varheader.Password,varheader.Channel, ChatEnterChannelReturnCode.Denied);
            var oldchannel = connection.CourentChannel;

            if (!channels.Any(c => c.Channelname.Value == varheader.Channel))
            {
                eventargs.ReturnCode = ChatEnterChannelReturnCode.DeniedChannelNotAvailable;
            }
            else
            {
                ClientEnteringChannel.Invoke(this, eventargs);
            }

            SendMessage.Invoke( GenerateEnterChannelAckMessage(eventargs.ReturnCode), connection.ClientIP);

            connection.CourentChannel = eventargs.Channel;
            ChannelChanged.Invoke(oldchannel, connection.CourentChannel);
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