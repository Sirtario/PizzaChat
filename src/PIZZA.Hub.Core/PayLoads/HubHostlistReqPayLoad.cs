using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Hub.Core.PayLoads
{
    public class HubHostlistReqPayLoad : HubPayLoad
    {
        private byte _flags = new byte();

        public HubHostlistReqPayLoad()
        { }

        private HubHostlistReqPayLoad(byte[] bytes)
        {
            _flags = bytes[0];
        }

        public bool RequestServers
        {
            get
            {
                return (_flags & 1) == 1;
            }
            set
            {
                _flags = (byte)(_flags & 254);

                if (value)
                {
                    _flags = (byte)(_flags | 1);
                }
            }
        }

        public bool RequestClients
        {
            get
            {
                return (_flags & 2) == 2;
            }
            set
            {
                _flags = (byte)(_flags & 253);

                if (value)
                {
                    _flags = (byte)(_flags | 2);
                }
            }
        }

        public bool IgnoreRequiredPassword
        {
            get => (_flags & 4) == 4;
            set
            {
                _flags = (byte)(_flags & 251);
               
                if(value)
                {
                    _flags = (byte)(_flags | 4);
                }
            }
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
