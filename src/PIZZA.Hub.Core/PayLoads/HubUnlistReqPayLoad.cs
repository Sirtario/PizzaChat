using System;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;

namespace PIZZA.Hub.Core.PayLoads
{
    public class HubUnlistReqPayLoad : HubPayLoad
    {
        private PIZZAInt _hostIdentifier = new PIZZAInt();

        public int Hostidentifier
        {
            get
            {
                return _hostIdentifier.Value;
            }
            set
            {
                _hostIdentifier.Value = value;
            }
        }

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
