using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIZZA.Core;

namespace PIZZA.Hub.Core
{
    public class HubMessage
    {
        public HubHeader Header { get; private set; }
        public HubPayLoad PayLoad { get; private set; }

        public HubMessage(HubPacketTypes type, HubPayLoad PayLoad)
        {
            this.PayLoad = PayLoad;

            PIZZAInt3 payLoadLength = new PIZZAInt3() { Value = this.PayLoad.GetBytes().Length };

            Header = new HubHeader(1 ,type, payLoadLength); 
        }

        public byte[] GetBytes()
        {
            var bytes = PayLoad.GetBytes();
            var headerBytes = Header.GetBytes(bytes.Length);

            bytes = headerBytes.Concat(bytes).ToArray();

            return bytes;
        }
    }
}
