using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Hub.Core
{
    public class TcpDelegate
    {
        public static bool IsPIZZAHubMessageComplete(byte[] bytes)
        {
            HubMessage message;

            try
            {
                message = HubMessageFactory.GetMessage(bytes);
            }
            catch
            {
                return false;
            }

            return message.Header.PayloadLength >= message.PayLoad.GetBytes().Length;
        }
    }
}
