using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Chat.Core
{
    public enum ChatEnterChannelReturnCode
    {
        Accepted,
        Denied,
        DeniedClientBannedFromChannel,
        DeniedBadPassword,
        DeniedChannelFull,
        DeniedClientNotWhitelisted,
        DeniedInternalError,
        DeniedMISC,
        DeniedChannelNotAvailable
    }
}
