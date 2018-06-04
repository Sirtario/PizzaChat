using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Chat.Core
{
    public class ChatPayload
    {
        public virtual byte[] GetBytes()
        {
            return new byte[0];
        }
    }
}
