using System;
using System.Collections.Generic;
using System.Net;
using PIZZA.Chat.Core;

namespace PIZZA.Chat.Server
{
    internal class PublishManager
    {
        public event Action<PizzaChatMessage, IPEndPoint> SendMessage;        

        internal void Publish(PizzaChatMessage message, ChatClientConnection myconnection, List<ChatClientConnection> clientsInMyChannel)
        {
            var varheader = message.VariableHeader as ChatVarHeaderPublish;
            //reset name so there is no way to manilulate the names
            varheader.SenderName = myconnection.ClientID;

            if (varheader.WhisperTarget != string.Empty)
            {
                foreach (var client in clientsInMyChannel)
                {
                    if (client.ClientID == varheader.WhisperTarget)
                    {

                        SendMessage.Invoke(message, client.ClientIP);
                    }
                }
            }

            foreach (var client in clientsInMyChannel)
            {
                SendMessage.Invoke(message, client.ClientIP);
            }
        }
    }
}