using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIZZA.Hub.Core.PayLoads;

namespace PIZZA.Hub.Core
{
    public static class HubMessageFactory
    {
        public static HubMessage GetMessage(HubPacketTypes type)
        {
            switch (type)
            {
                case HubPacketTypes.SERVERENLISTREQ:
                    return new HubMessage(HubPacketTypes.SERVERENLISTREQ, new HubServerenlistreqPayLoad());
                case HubPacketTypes.CLIENTENLISTREQ:
                    return new HubMessage(HubPacketTypes.CLIENTENLISTREQ, new HubClientEnlistReqPayLoad());
                case HubPacketTypes.ENLISTACK:
                    return new HubMessage(HubPacketTypes.ENLISTACK, new HubEnlistAckPayLoad());
                case HubPacketTypes.HOSTLISTREQ:
                    return new HubMessage(HubPacketTypes.HOSTLISTREQ, new HubHostlistReqPayLoad());
                case HubPacketTypes.HOSTLISTDAT:
                    return new HubMessage(HubPacketTypes.HOSTLISTDAT, new HubHostlistDatPayLoad());
                case HubPacketTypes.HOSTAVAILABLEREQ:
                    return new HubMessage(HubPacketTypes.HOSTAVAILABLEREQ, new HubHostavailableReqPayLoad());
                case HubPacketTypes.HOSTAVAILABLEDAT:
                    return new HubMessage(HubPacketTypes.HOSTAVAILABLEDAT, new HubHostavailableDatPayLoad());
                case HubPacketTypes.UNLISTREQ:
                    return new HubMessage(HubPacketTypes.UNLISTREQ, new HubUnlistReqPayLoad());
                case HubPacketTypes.UNLISTACK:
                    return new HubMessage(HubPacketTypes.UNLISTACK, new HubUnlistAckPayLoad());
                case HubPacketTypes.PING:
                    return new HubMessage(HubPacketTypes.PING, new HubPingPayLoad());
                case HubPacketTypes.PINGACK:
                    return new HubMessage(HubPacketTypes.PINGACK, new HubPingAckPayLoad());
                default:
                    throw new NotSupportedException($"Unknown packet type {type.ToString()} couldn't create HubMessage!");
            }
        }

        public static HubMessage GetMessage(byte[] messagebytes)
        {
            return new HubMessage(messagebytes);
        }
    }
}
