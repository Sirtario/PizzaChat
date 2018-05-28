using System;
using System.Collections.Generic;
using System.Text;

namespace Pizza.Chat.Core
{
    public class Fixedheader
    {
        public Packettypes PacketType { get; set; }

        public long RemainingLength { get; set; }
    }
}
