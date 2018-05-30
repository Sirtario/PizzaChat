using PIZZA.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PIZZA.Chat.Core
{
    internal class PIZZAChannel
    {
        public PIZZAChannel(byte[] bytes)
        {
            var list = bytes.ToList();

            Channelname = PIZZAString.FromBytes(list.ToArray());
            list.RemoveRange(0, Channelname.Length.Value);

            HasPassword = list[0];
            list.RemoveAt(0);

            if (list.Count != 0)
            {
                throw new Exception();
            }
        }

        public int Length => GetBytes().Length;

        public PIZZAString Channelname { get; set; }

        public byte HasPassword { get; set; }

        internal static PIZZAChannel FromBytes(byte[] v)
        {
            return new PIZZAChannel(v);
        }

        internal byte[] GetBytes()
        {
            var bytes = new List<byte>();

            bytes.AddRange(Channelname.GetBytes());
            bytes.Add(HasPassword);

            return bytes.ToArray();
        }
    }
}