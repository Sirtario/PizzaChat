using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Hub.Core
{
    class HubHeader
    {
        public byte[] ProtocollName;
        public byte ProtocollVersion;
        public byte[] PacketType;
    }
}
