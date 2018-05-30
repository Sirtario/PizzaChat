using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Hub.Core.PayLoads
{
    class HubUnlistAckPayLoad : HubPayLoad
    {
        public HubUnlistAckPayLoad()
        { }

        private HubUnlistAckPayLoad(byte[] bytes)
        { }

        public static HubUnlistAckPayLoad FromBytes(byte[] bytes)
        {
            HubUnlistAckPayLoad result = new HubUnlistAckPayLoad(bytes);

            return result;
        }

        public byte[] GetBytes()
        {
            return new byte[] { };
        }
    }
}
