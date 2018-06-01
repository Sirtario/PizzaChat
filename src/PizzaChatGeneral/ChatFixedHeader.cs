using PIZZA.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Chat.Core
{
    public class ChatFixedHeader
    {
        private PIZZAInt5 _remainingLength = new PIZZAInt5();

        public ChatFixedHeader(Packettypes packetType)
        {
            PacketType = packetType;
        }

        private ChatFixedHeader(byte[] bytes)
        {
            var list = new List<byte>(bytes);
            Protokollname = list.GetRange(0, 6).ToArray();
            list.RemoveRange(0, 6);

            ProtokollVersion = list[0];
            list.RemoveAt(0);

            PacketType = (Packettypes)list[0];
            list.RemoveAt(0);

            _remainingLength = PIZZAInt5.FromBytes(list.GetRange(0,5).ToArray());
        }

        public Packettypes PacketType { get; set; }

        public long RemainingLength { get { return _remainingLength.Value; } set { _remainingLength.Value = value; } }

        public Byte[] Protokollname { get; private set; } = Encoding.UTF8.GetBytes("PIZZAC");

        /// <summary>
        /// Version of PIZZAChat Protokoll
        /// </summary>
        public byte ProtokollVersion { get; private set; } = 1;

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
        public byte[] GetBytes(int remainingLength)
        {
            _remainingLength.Value = remainingLength;

            var bytes = new List<byte>();

            bytes.AddRange(Protokollname);
            bytes.Add(ProtokollVersion);
            bytes.Add((byte)PacketType);
            bytes.AddRange(_remainingLength.GetBytes());

            return bytes.ToArray();
        }
    }
}
