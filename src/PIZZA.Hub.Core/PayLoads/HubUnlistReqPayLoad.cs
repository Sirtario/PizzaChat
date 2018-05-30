using System;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;

namespace PIZZA.Hub.Core.PayLoads
{
    public class HubUnlistReqPayLoad : HubPayLoad
    {
        private PIZZAInt _hostIdentifier = new PIZZAInt();

        public HubUnlistReqPayLoad()
        { }

        private HubUnlistReqPayLoad(byte[] bytes)
        {
            _hostIdentifier = PIZZAInt.FromBytes(bytes);
        }

        public static HubUnlistReqPayLoad FromBytes(byte[] bytes)
        {
            HubUnlistReqPayLoad result = new HubUnlistReqPayLoad(bytes);

            return result;
        }

        public byte[] GetBytes()
        {
            return _hostIdentifier.GetBytes();
        }
    }
}
