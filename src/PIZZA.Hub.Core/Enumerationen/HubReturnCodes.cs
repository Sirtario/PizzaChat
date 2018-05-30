using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Hub.Core.Enumerationen
{
    public enum HubReturnCodes
    {
        ACCEPTED = 0,
        DENIED_BAD_PASSWORD = 1,
        DENIED_ALREDY_LISTED = 2,
        DENIED_BAD_PROTOCOLLVERSION = 3,
        DENIED_NOT_SUPPORTED = 4,
        DENIED_HOST_NOT_AVAILABLE = 5,
        DENIED_MISC = 6
    }
}
