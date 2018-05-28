using System;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;

namespace PIZZA.Hub.Core
{
    class HubHeader
    {
        private byte[] _protocollName;
        private byte _protocollVersion;

        public HubPacketTypes PacketType { get; set; }

        public PIZZAInt3 PayloadLength { get; set; }

        public void SetProtocollVersion(byte Version)
        {
            _protocollVersion = Version;
        }

        public byte GetProtocollVersion()
        {
            return _protocollVersion;
        }

    }
}
