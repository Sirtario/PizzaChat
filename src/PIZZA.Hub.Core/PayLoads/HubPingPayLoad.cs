using System;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;

namespace PIZZA.Hub.Core.PayLoads
{
    public class HubPingPayLoad : HubPayLoad
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

        public HubPingPayLoad()
        { }

        private HubPingPayLoad(byte[] bytes)
        {
            _hostIdentifier = PIZZAInt.FromBytes(bytes);
        }

        public static HubPingPayLoad FromBytes(byte[] bytes)
        {
            HubPingPayLoad result = new HubPingPayLoad(bytes);

            return result;
        }

        public byte[] GetBytes()
        {
            return _hostIdentifier.GetBytes();
        }
    }
}
