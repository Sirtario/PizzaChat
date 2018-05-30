using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Hub.Core.PayLoads
{
    class HubHostlistReqPayLoad : HubPayLoad
    {
        private byte _flags;


        private HubHostlistReqPayLoad(byte[] bytes)
        {
            _flags = bytes[0];
        }

        public static HubHostlistReqPayLoad FromBytes(byte[] bytes)
        {
            HubHostlistReqPayLoad result = new HubHostlistReqPayLoad(bytes);

            return result;
        }

        public byte[] GetBytes()
        {
            byte[] result = new byte[] { _flags };

            return result;
        }
    }
}
