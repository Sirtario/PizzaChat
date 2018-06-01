using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Chat.Core
{
    public static class TcpDelegate
    {
        public static bool IsPIZZAChatMessageComplete(byte[] bytes)
        {
            PizzaChatMessage message;

            try
            {
                message = PizzaChatMessage.FromBytes(bytes);
            }
            catch
            {
                return false;
            }

            return message.FixedHeader.RemainingLength >= message.Payload.GetBytes().Length + message.VariableHeader.GetBytes().Length;
        }
    }
}
