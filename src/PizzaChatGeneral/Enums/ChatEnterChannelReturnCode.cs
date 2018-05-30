using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Chat.Core
{
    public enum ChatEnterChannelReturnCode
    {
        Acceptded,
        Denied,
        DeniedClientBannedFromChannel,
        DeniedBadPassword,
        DeniedChannelFull,
        DeniedClientNotWhitelisted,
        DeniedInternalError,
        DeniedMISC
    }
}
