using PIZZA.Hub.Core.Enumerationen;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Hub.Client
{
    public class EnlistResult
    {
        public EnlistResult(HubReturnCodes returncode, int hostIdentifier, int pinginterval)
        {
            Returncode = returncode;
            HostIdentifier = hostIdentifier;
            Pinginterval = pinginterval;
        }

        public HubReturnCodes Returncode { get; }
        public int HostIdentifier { get; }
        public int Pinginterval { get; }
    }
}
