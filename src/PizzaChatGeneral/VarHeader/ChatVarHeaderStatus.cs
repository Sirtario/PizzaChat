using PIZZA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIZZA.Chat.Core
{
    public class ChatVarHeaderStatus : ChatVariableHeader
    {
        private PIZZAString _courentChannel = new PIZZAString();

        public ChatVarHeaderStatus()
        {
        }

        private ChatVarHeaderStatus(byte[] bytes)
        {
            var list = bytes.ToList();

            list.RemoveRange(0, 5);

            _courentChannel = PIZZAString.FromBytes(list.ToArray());
            list.RemoveRange(0, _courentChannel.GetBytes().Length);

            ChannelPower = list[0];
            list.RemoveAt(0);

            ClientCount = list[0];
            list.RemoveAt(0);

            ChannelCount = list[0];
            list.RemoveAt(0);
        }

        /// <summary>
        /// Talkpower the Client has in the Courent Cannel
        /// </summary>
        public byte ChannelPower { get;  set; }

        /// <summary>
        /// Returns the number of Clients that are connected in the Courent Channel
        /// </summary>
        public byte ClientCount { get;  set; }

        /// <summary>
        /// the number of channels oh the server
        /// </summary>
        public byte ChannelCount { get;  set; }

        /// <summary>
        /// Name of the Courent Channel
        /// </summary>
        public string CurrentChannel {
            get
            {
                return _courentChannel.Value;
            }
            set
            {
                _courentChannel.Value = value;
            }
        }

        /// <summary>
        /// Returns a new VarHeaderStatus Variable header from Byte[]
        /// </summary>
        /// <param name="bytes">byte[] to be deserialized</param>
        /// <returns></returns>
        public static ChatVarHeaderStatus FromBytes(byte[]bytes)
        {
            return new ChatVarHeaderStatus(bytes);
        }

        /// <summary>
        /// returns a bytearray that represents the VarHeader
        /// </summary>
        /// <returns></returns>
        protected override byte[] GetBytes()
        {
            var bytes = new List<byte>();

            bytes.AddRange(_courentChannel.GetBytes());
            bytes.Add(ChannelPower);
            bytes.Add(ClientCount);
            bytes.Add(ChannelCount);

            return bytes.ToArray();
        }
    }
}
