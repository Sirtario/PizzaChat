using System;
using System.Collections.Generic;
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
                    return new HubMessage(HubPacketTypes.SERVERENLISTREQ, new HubServerunlistreqPayLoad());
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
                case HubPacketTypes.UNLISTTACK:
                    return new HubMessage(HubPacketTypes.UNLISTTACK, new HubUnlistAckPayLoad());
                case HubPacketTypes.PING:
                    return new HubMessage(HubPacketTypes.PING, new HubPingPayLoad());
                case HubPacketTypes.PINGACK:
                    return new HubMessage(HubPacketTypes.PINGACK, new HubPingAckPayLoad());
                default:
                    throw new NotSupportedException($"Unknown packet type {type.ToString()} couldn't create HubMessage!");
            }
        }

        public static HubMessage GetMessage(byte[] bytes)
        {
            var header = HubHeader.FromBytes(bytes);

            switch (header.PacketType)
            {
                case HubPacketTypes.SERVERENLISTREQ:
                    return new HubMessage(HubPacketTypes.SERVERENLISTREQ, HubServerunlistreqPayLoad.FromBytes(bytes));
                case HubPacketTypes.CLIENTENLISTREQ:
                    return new HubMessage(HubPacketTypes.CLIENTENLISTREQ, HubClientEnlistReqPayLoad.FromBytes(bytes));
                case HubPacketTypes.ENLISTACK:
                    return new HubMessage(HubPacketTypes.ENLISTACK, HubEnlistAckPayLoad.FromBytes(bytes));
                case HubPacketTypes.HOSTLISTREQ:
                    return new HubMessage(HubPacketTypes.HOSTLISTREQ, HubHostlistReqPayLoad.FromBytes(bytes));
                case HubPacketTypes.HOSTLISTDAT:
                    return new HubMessage(HubPacketTypes.HOSTLISTDAT, HubHostlistDatPayLoad.FromBytes(bytes));
                case HubPacketTypes.HOSTAVAILABLEREQ:
                    return new HubMessage(HubPacketTypes.HOSTAVAILABLEREQ, HubHostavailableReqPayLoad.FromBytes(bytes));
                case HubPacketTypes.HOSTAVAILABLEDAT:
                    return new HubMessage(HubPacketTypes.HOSTAVAILABLEDAT, HubHostavailableDatPayLoad.FromBytes(bytes));
                case HubPacketTypes.UNLISTREQ:
                    return new HubMessage(HubPacketTypes.UNLISTREQ, HubUnlistReqPayLoad.FromBytes(bytes));
                case HubPacketTypes.UNLISTTACK:
                    return new HubMessage(HubPacketTypes.UNLISTTACK, HubUnlistAckPayLoad.FromBytes(bytes));
                case HubPacketTypes.PING:
                    return new HubMessage(HubPacketTypes.PING, HubPingPayLoad.FromBytes(bytes));
                case HubPacketTypes.PINGACK:
                    return new HubMessage(HubPacketTypes.PINGACK, HubPingAckPayLoad.FromBytes(bytes));
                default:
                    throw new NotSupportedException($"Unknown packet type {header.PacketType.ToString()} couldn't create HubMessage!");
            }
        }
    }
}
