using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIZZA.Core;
using PIZZA.Hub.Core.PayLoads;

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

            Header = new HubHeader(type); 
        }

        public HubMessage(byte[] messagebytes)
        {
            var bytes = messagebytes.ToList();

            Header = HubHeader.FromBytes(bytes.ToArray());
            bytes.RemoveRange(0, 11);

            switch (Header.PacketType)
            {
                case HubPacketTypes.SERVERENLISTREQ:
                    PayLoad = HubServerenlistreqPayLoad.FromBytes(bytes.ToArray());
                    break;
                case HubPacketTypes.CLIENTENLISTREQ:
                    PayLoad = HubClientEnlistReqPayLoad.FromBytes(bytes.ToArray());
                    break;
                case HubPacketTypes.ENLISTACK:
                    PayLoad = HubEnlistAckPayLoad.FromBytes(bytes.ToArray());
                    break;
                case HubPacketTypes.HOSTLISTREQ:
                    PayLoad = HubHostlistReqPayLoad.FromBytes(bytes.ToArray());
                    break;
                case HubPacketTypes.HOSTLISTDAT:
                    PayLoad = HubHostlistDatPayLoad.FromBytes(bytes.ToArray());
                    break;
                case HubPacketTypes.HOSTAVAILABLEREQ:
                    PayLoad = HubHostavailableReqPayLoad.FromBytes(bytes.ToArray());
                    break;
                case HubPacketTypes.HOSTAVAILABLEDAT:
                    PayLoad = HubHostavailableDatPayLoad.FromBytes(bytes.ToArray());
                    break;
                case HubPacketTypes.UNLISTREQ:
                    PayLoad = HubUnlistReqPayLoad.FromBytes(bytes.ToArray());
                    break;
                case HubPacketTypes.UNLISTACK:
                    PayLoad = HubUnlistAckPayLoad.FromBytes(bytes.ToArray());
                    break;
                case HubPacketTypes.PING:
                    PayLoad = HubPingPayLoad.FromBytes(bytes.ToArray());
                    break;
                case HubPacketTypes.PINGACK:
                    PayLoad = HubPingAckPayLoad.FromBytes(bytes.ToArray());
                    break;
                default:
                    throw new NotImplementedException($"unknown Packettype {Header.PacketType.ToString()}");
            }
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
