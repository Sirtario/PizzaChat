using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;

namespace PIZZA.Hub.Core
{
    public class HubHeader
    {
        private byte[] _protocollName;

        private byte _protocollVersion;

        private HubPacketTypes _packetType;

        private PIZZAInt3 _payloadLength;

        public byte[] ProtocollName => _protocollName;

        public byte ProtocollVersion => _protocollVersion;

        public HubPacketTypes PacketType => _packetType;

        public PIZZAInt3 PayloadLength => _payloadLength;

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

        internal byte[] GetBytes(int payloadLength)
        {
            var bytes = _protocollName;

            bytes = bytes.Concat(new byte[] { _protocollVersion })
                .Concat(new byte[] { (byte)_packetType })
                .Concat(new byte[] { (byte)payloadLength })
                .ToArray();

            return bytes;
        }

        public HubHeader(byte protocollversion, HubPacketTypes type, PIZZAInt3 payloadlength)
        {
            _protocollName = Encoding.UTF8.GetBytes("PIZZAH");
            _protocollVersion = protocollversion;
            _packetType = type;
            _payloadLength = payloadlength;
        }

        public static HubHeader FromBytes(byte[] bytes)
        {
            var result = new HubHeader(bytes);

            return result;
        }
    }
}
