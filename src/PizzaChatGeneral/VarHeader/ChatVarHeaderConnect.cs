using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Chat.Core
{
    class ChatVarHeaderConnect : ChatVariableHeader
    {
        private ChatVarHeaderConnect(byte[] bytes)
        {
            //TODO: FromBytes Logic 
        }

        public ChatVarHeaderConnect()
        {

        }

        PIZZAString ClientID { get; set; }

        PIZZAString Password { get; set; }

        public static FromBytes(byte[] bytes)
        {
            return new ChatVarHeaderConnect(bytes);
        }

        public byte[] GetBytes()
        {
            var bytes = new List<byte>();

            bytes.AddRange(Lenght.GetBytes());
            bytes.AddRange(ClientID.GetBytes());
            bytes.AddRange(Password.GetBytes());

            return bytes.ToArray();
        }

    }
}
