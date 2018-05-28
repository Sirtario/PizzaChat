using PIZZA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIZZA.Chat.Core
{
    class ChatVarHeaderConnAck : ChatVariableHeader
    {
        public ChatVarHeaderConnAck()
        {
        }

        private ChatVarHeaderConnAck(byte[] bytes)
        {
            var list = bytes.ToList();

            Lenght = PIZZAInt5.FromBytes(list.ToArray());
            list.RemoveRange(0, 5);

            Returncode = (ChatConnectReturncode)list[0];
            list.RemoveAt(0);

            CommunicationMode = list[0];
            list.RemoveAt(0);

            PingIntervall = list[0];
            list.RemoveAt(0);

            if (list.Count != 0)
            {
                throw new Exception();
            }
        }

        public ChatConnectReturncode Returncode { get; private set; }
        public byte CommunicationMode { get; private set; }
        public byte PingIntervall { get; private set; }

        public byte[] GetBytes()
        {
            var bytes = new List< byte>();

            bytes.AddRange(Lenght.GetBytes());
            bytes.Add((byte)Returncode);
            bytes.Add(CommunicationMode);
            bytes.Add(PingIntervall);

            return bytes.ToArray();
        }

        /// <summary>
        /// returns a new ConnAck Variable header
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>A new Variable header </returns>
        public static ChatVarHeaderConnAck FromBytes(byte[] bytes)
        {
            return new ChatVarHeaderConnAck(bytes);
        }
    }
}
