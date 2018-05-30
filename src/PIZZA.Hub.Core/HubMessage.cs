using System;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;

namespace PIZZA.Hub.Core
{
    public class HubMessage
    {
        private HubHeader _header;
        private HubPacketTypes _type;
        private HubPayLoad _payLoad;

        public HubMessage(HubPacketTypes type, HubPayLoad PayLoad)
        {
            _payLoad = PayLoad;

            PIZZAInt3 payLoadLength = new PIZZAInt3() { Value = _payLoad.GetBytes().Length };

            _header = new HubHeader(1 ,type, payLoadLength); 
        }

    }
}
