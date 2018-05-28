using PIZZA.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Chat.Core
{
    public class ChatFixedHeader
    {
        private PIZZAInt5 _remainingLength;

        public ChatFixedHeader(Packettypes packetType)
        {
            PacketType = packetType;
        }

        private ChatFixedHeader(byte[] bytes)
        {
            var list = new List<byte>(bytes);

            list.RemoveRange(0, 7);

            PacketType = (Packettypes)list[0];
            list.RemoveAt(0);

            RemainingLength = PIZZAInt5.FromBytes(list.ToArray());
        }

        public Packettypes PacketType { get; set; }

        public long RemainingLength { get { return _remainingLength.Value; } set { _remainingLength.Value = value; } }

        public Byte[] Protokollname { get; private set; } = Encoding.UTF8.GetBytes("PIZZAC");

        /// <summary>
        /// Version of PIZZAChat Protokoll
        /// </summary>
        public byte ProtokollVersion { get; } = 1;

        /// <summary>
        /// returns a new Fixedheader from teh bytearray
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ChatFixedHeader FromBytes(byte[] bytes)
        {
            return new ChatFixedHeader(bytes);
        }

        /// <summary>
        /// returns the Fixed Header as bytearray
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            var bytes = new List<byte>();

            bytes.AddRange(Protokollname);
            bytes.Add(ProtokollVersion);
            bytes.Add((byte)PacketType);
            bytes.AddRange(RemainingLength.GetBytes());

            return bytes.ToArray();
        }
    }
}
