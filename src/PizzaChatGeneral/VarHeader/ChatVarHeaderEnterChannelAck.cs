using PIZZA.Chat.Core;
using PIZZA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIZZA.Chat.Core
{
    public class ChatVarHeaderEnterChannelAck : ChatVariableHeader
    {
        public ChatVarHeaderEnterChannelAck()
        {
        }

        private ChatVarHeaderEnterChannelAck(byte[] bytes)
        {
            var list = bytes.ToList();

            Lenght = PIZZAInt5.FromBytes(list.ToArray());
            list.RemoveRange(0, 5);

            ReturnCode = (ChatEnterChannelReturnCode)list[0];
            list.RemoveAt(0);
        }

        /// <summary>
        /// the returncode of the enter channel operation
        /// </summary>
        public ChatEnterChannelReturnCode ReturnCode
        {
            get;
            private set;
        } 

        /// <summary>
        /// gets the Varheader from the byte[]
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ChatVarHeaderEnterChannelAck FromBytes(byte[] bytes)
        {
            return new ChatVarHeaderEnterChannelAck(bytes);
        }

        /// <summary>
        /// gets the byte[] that resembles the Var header
        /// </summary>
        /// <returns></returns>
        public override byte[] GetBytes()
        {
            var bytes = new List<byte>();

            bytes.AddRange(Lenght.GetBytes());
            bytes.Add((byte)ReturnCode);

            return bytes.ToArray();
        }
    }
}
