using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;

namespace PIZZA.Hub.Core.PayLoads
{
    class HubHostavailableReqPayLoad : HubPayLoad
    {
        private PIZZAString _hostname = new PIZZAString();

        public HubHostavailableReqPayLoad()
        { }

        private HubHostavailableReqPayLoad(byte[] bytes)
        {
            _hostname = PIZZAString.FromBytes(bytes);
        }

        public static HubHostavailableReqPayLoad FromBytes(byte[] bytes)
        {
            HubHostavailableReqPayLoad result = new HubHostavailableReqPayLoad(bytes);

            return result;
        }

        public byte[] GetBytes()
        {
            return _hostname.GetBytes();
        }
    }
}
