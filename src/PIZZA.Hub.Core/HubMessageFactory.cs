using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Hub.Core
{
    public static class HubMessageFactory
    {
        public static HubMessage GetMessage(HubPacketTypes type)
        {
            switch (type)
            {
                case HubPacketTypes.SERVERENLISTREQ:
                    return new HubMessage(HubPacketTypes.SERVERENLISTREQ);
                case HubPacketTypes.CLIENTENLISTREQ:
                    return new HubMessage(HubPacketTypes.CLIENTENLISTREQ);
                case HubPacketTypes.ENLISTACK:
                    return new HubMessage(HubPacketTypes.ENLISTACK);
                case HubPacketTypes.HOSTLISTREQ:
                    return new HubMessage(HubPacketTypes.HOSTLISTREQ);
                case HubPacketTypes.HOSTLISTDAT:
                    return new HubMessage(HubPacketTypes.HOSTLISTDAT);
                case HubPacketTypes.HOSTAVAILABLEREQ:
                    return new HubMessage(HubPacketTypes.HOSTAVAILABLEREQ);
                case HubPacketTypes.HOSTAVAILABLEDAT:
                    return new HubMessage(HubPacketTypes.HOSTAVAILABLEDAT);
                case HubPacketTypes.UNLISTREQ:
                    return new HubMessage(HubPacketTypes.UNLISTREQ);
                case HubPacketTypes.UNLISTTACK:
                    return new HubMessage(HubPacketTypes.UNLISTTACK);
                case HubPacketTypes.PING:
                    return new HubMessage(HubPacketTypes.PING);
                case HubPacketTypes.PINGACK:
                    return new HubMessage(HubPacketTypes.PINGACK);
                default:
                    throw new NotSupportedException($"Unknown packet type {type.ToString()} could'nt create HubMessage!");
            }
        }
    }
}
