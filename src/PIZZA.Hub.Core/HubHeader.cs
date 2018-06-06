using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;

namespace PIZZA.Hub.Core
{
    public class HubHeader
    {
        private PIZZAInt3 _payloadLength = new PIZZAInt3();
        private byte[] _protocollName = new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x48};
        private byte _protocollVersion = 1;
        private HubPacketTypes _packetType;

        public HubHeader( HubPacketTypes type)
        {
            _packetType = type;
           
        }

        private HubHeader(byte[] bytes)
        {
            List<byte> tmp = bytes.ToList();
            _protocollName = tmp.GetRange(0, 6).ToArray();
            tmp.RemoveRange(0, 6);
            _protocollVersion = tmp[0];
            tmp.RemoveAt(0);
            _packetType = (HubPacketTypes) tmp[0];
            tmp.RemoveAt(0);
            _payloadLength = PIZZAInt3.FromBytes(tmp.ToArray());
            tmp.Clear();
        }

        public byte[] ProtocollName => _protocollName;

        public byte ProtocollVersion => _protocollVersion;

        public HubPacketTypes PacketType => _packetType;

        public int PayloadLength
        {
            get
            {
                return _payloadLength.Value;
            }
        }

        public byte[] GetBytes(int payloadLength)
        {
            byte[] bytes = ProtocollName;

            _payloadLength.Value = payloadLength;

            bytes = bytes.Concat(new byte[] { ProtocollVersion })
                .Concat(new byte[] { (byte)PacketType })
                .Concat(_payloadLength.GetBytes())
                .ToArray();

            return bytes;
        }

        public static HubHeader FromBytes(byte[] bytes)
        {
            HubHeader result = new HubHeader(bytes);

            return result;
        }
    }
}
