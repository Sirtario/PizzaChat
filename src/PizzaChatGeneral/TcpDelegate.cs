using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Chat.Core
{
    public static class TcpDelegate
    {
        public static bool IsPIZZAChatMessageComplete(byte[] bytes)
        {
            if(bytes.Length < 13)
            {
                return false;
            }

            var fixedHeader = ChatFixedHeader.FromBytes(bytes);

            return fixedHeader.RemainingLength == bytes.Length - 13;
        }
    }
}
