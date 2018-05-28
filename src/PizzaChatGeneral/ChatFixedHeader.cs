using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Chat.Core
{
    public class ChatFixedHeader
    {
        public ChatFixedHeader(Packettypes packetType)
        {
            PacketType = packetType;
        }

        private ChatFixedHeader(byte[] bytes)
        {
            //TODO: FromBytes Logik einfügen
        }

        public Packettypes PacketType { get; set; }

        public PIZZAInt5 RemainingLength { get; set; }

        public Byte[] Protokollname { get; } = Encoding.UTF8.GetBytes("PIZZAC");

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
