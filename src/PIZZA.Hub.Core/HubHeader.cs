using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;

namespace PIZZA.Hub.Core
{
    class HubHeader
    {
        private byte[] _protocollName;

        private byte _protocollVersion;

        private HubPacketTypes _packetType;

        private PIZZAInt3 _payloadLength;

        public byte[] ProtocollName => _protocollName;

        public byte ProtocollVersion => _protocollVersion;

        public HubPacketTypes PacketTypes => _packetType;

        public PIZZAInt3 PayloadLegnth => _payloadLength;

        private HubHeader(byte[] bytes)
        {
            List<byte> tmp = bytes.ToList();
            _protocollName = tmp.GetRange(0, 6).ToArray();
            tmp.RemoveRange(0, 6);
            _protocollVersion = tmp[0];
            tmp.RemoveAt(0);
            _packetType = (HubPacketTypes) tmp[0];
            tmp.RemoveAt(0);
            _payloadLength = PIZZAInt3.FromBytes(tmp.GetRange(0, 3).ToArray());
            tmp.Clear();
        }  

        public static HubHeader FromBytes(byte[] bytes)
        {
            HubHeader result = new HubHeader(bytes);
            return result;
        }
    }
}
