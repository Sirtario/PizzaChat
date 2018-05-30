using System;

namespace PIZZA.Chat.Core
{
    public enum Packettypes
    {
        CONNECT = 1,
        CONNACK = 2,
        GETSTATUS = 3,
        STATUS = 4,
        PING = 5,
        PINGRESP = 6,
        ENTERCHANNEL = 7,
        ENTERCHANNELACK = 8,
        DISCONNECT = 9,
        PUBLISH = 10
    }
}