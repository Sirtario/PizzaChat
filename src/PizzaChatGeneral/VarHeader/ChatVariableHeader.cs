using PIZZA.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Chat.Core
{
    public class ChatVariableHeader
    {
        public PIZZAInt5 Lenght => new PIZZAInt5() { Value = GetBytes().Length };

        public virtual byte[] GetBytes()
        {
            return new byte[0];
        }
    }
}
