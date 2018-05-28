using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PIZZA.Core
{
    public class PIZZAInt
    {
        private byte[] _value = new byte[2];

        private PIZZAInt(byte[] value)
        {
            if (value.Length < 2)
            {
                throw new ArgumentOutOfRangeException("value is too short");
            }

            _value = PIZZAIntHelper.GetNBytes(value, 2);
        }

        public PIZZAInt()
        {
            Value = 0;
        }

        public int Value
        {
            get
            {
                return (int)PIZZAIntHelper.GetValueFromBytes(_value);
            }
            set
            {
                _value = PIZZAIntHelper.GetBytesFromLong((long)value, 2);
            }
        }

        public byte[] GetBytes()
        {
            return _value;
        }

        public static PIZZAInt FromBytes(byte[] value)
        {
            return new PIZZAInt(value);
        }
    }
}
