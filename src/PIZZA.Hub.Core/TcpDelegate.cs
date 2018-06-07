using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Hub.Core
{
    public class TcpDelegate
    {
        public static bool IsPIZZAHubMessageComplete(byte[] bytes)
        {
            if(bytes.Length < 11)
            {
                return false;
            }

            var header = HubHeader.FromBytes(bytes);

            return header.PayloadLength == bytes.Length - 11;
        }
    }
}
