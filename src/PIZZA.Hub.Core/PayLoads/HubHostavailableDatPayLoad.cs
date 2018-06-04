using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Hub.Core.PayLoads
{
    public class HubHostavailableDatPayLoad : HubPayLoad
    {
        private byte _isAvailable = new byte();

        public HubHostavailableDatPayLoad()
        { }

        public bool IsAvailable
        {
            get
            {
                return _isAvailable >= 1;
            }
            set
            {
                _isAvailable = (byte)(value ? 1 : 0);
            }
        }

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
