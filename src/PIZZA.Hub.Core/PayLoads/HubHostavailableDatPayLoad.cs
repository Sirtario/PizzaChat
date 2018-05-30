using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Hub.Core.PayLoads
{
    class HubHostavailableDatPayLoad : HubPayLoad
    {
        private byte _isAvailable = new byte();

        public HubHostavailableDatPayLoad()
        { }

        private HubHostavailableDatPayLoad(byte[] bytes)
        {
            _isAvailable = bytes[0];
        }

        public static HubHostavailableDatPayLoad FromBytes(byte[] bytes)
        {
            HubHostavailableDatPayLoad result = new HubHostavailableDatPayLoad(bytes);

            return result;
        }

        public byte[] GetBytes()
        {
            return new byte[] { _isAvailable };
        }
    }
}
