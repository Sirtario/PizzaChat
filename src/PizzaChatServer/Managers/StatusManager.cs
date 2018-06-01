using System;
using System.Collections.Generic;
using System.Net;
using PIZZA.Chat.Core;

namespace PIZZA.Chat.Server
{
    internal class StatusManager
    {
        public event Action<PizzaChatMessage, IPEndPoint> SendMessage;

        internal void RecieveGetStatus(ChatClientConnection connection, List<ChatClientConnection> users, List<PIZZAChannel> channels)
        {
            SendStatus(connection, users, channels);
        }

        internal void SendStatus(ChatClientConnection connection, List<ChatClientConnection> users, List<PIZZAChannel> channels)
        {
            var statusmessage = new PizzaChatMessage(Packettypes.STATUS);

            var varheader = statusmessage.VariableHeader as ChatVarHeaderStatus;
            var payload = statusmessage.Payload as ChatPayloadStatus;

            varheader.ChannelCount = (byte)channels.Count;
            varheader.ChannelPower = connection.Channelpower;
            varheader.ClientCount = (byte)users.Count;
            varheader.CurrentChannel = connection.CourentChannel;

            foreach (var channel in channels)
            {
                payload.AddChannel(channel);
            }

            foreach (var client in users)
            {
                payload.AddUser(client.ClientID);
            }

            SendMessage.Invoke(statusmessage, connection.ClientIP);
        }
    }
}