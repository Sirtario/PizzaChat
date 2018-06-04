using System;

namespace PIZZA.Hub.Core
{
    public enum HubPacketTypes
    {
        SERVERENLISTREQ = 1,
        CLIENTENLISTREQ = 2,
        ENLISTACK = 3,
        HOSTLISTREQ = 4,
        HOSTLISTDAT = 5,
        HOSTAVAILABLEREQ = 6,
        HOSTAVAILABLEDAT = 7,
        UNLISTREQ = 8,
        UNLISTACK = 9,
        PING = 10,
        PINGACK = 11
    }
}
