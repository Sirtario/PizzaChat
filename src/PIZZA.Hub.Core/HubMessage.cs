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

        public HubMessage(HubPacketTypes type)
        {
            _header = new HubHeader(1 ,type, payloadlength); 
        }

    }
}
