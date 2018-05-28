using System;

namespace PIZZA.Core
{
    public class PIZZAInt5
    {
        public long Value { get { return Bytes.ToLong(); } }

        public byte[] Bytes { get; private set; }
    }
}
