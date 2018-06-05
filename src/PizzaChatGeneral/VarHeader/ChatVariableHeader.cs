using PIZZA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIZZA.Chat.Core
{
    public class ChatVariableHeader
    {
        protected virtual byte[] GetBytes()
        {
            return new byte[0];
        }

        public byte[] GetBytes(bool ignoreLength = false)
        {
            var bytes = GetBytes().ToList();

            if(bytes.Count != 0 && !ignoreLength)
            {
                bytes.InsertRange(0, new PIZZAInt5() { Value = bytes.Count + 5 }.GetBytes());
            }

            return bytes.ToArray();
        }
    }
}
