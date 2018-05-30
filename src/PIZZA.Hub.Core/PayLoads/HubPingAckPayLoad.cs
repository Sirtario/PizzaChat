using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Hub.Core.PayLoads
{
    public class HubPingAckPayLoad : HubPayLoad
    {
        public HubPingAckPayLoad()
        { }

        private HubPingAckPayLoad(byte[] bytes)
        { }

        public static HubPingAckPayLoad FromBytes(byte[] bytes)
        {
            HubPingAckPayLoad result = new HubPingAckPayLoad(bytes);

            return result;
        }

        public byte[] GetBytes()
        {
            return new byte[] { };
        }
    }
}
