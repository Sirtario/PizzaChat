using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Chat.Core
{
    public static class TcpDelegate
    {
        public static bool IsPIZZAChatMessageComplete(byte[] bytes)
        {
            try
            {
                var package = PizzaChatMessage.FromBytes(bytes);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
