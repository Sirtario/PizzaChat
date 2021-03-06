using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Core
{
    public class PIZZAInt3
    {
        private byte[] _value = new byte[3];

        private PIZZAInt3(byte[] value)
        {
            if(value.Length < 3)
            {
                throw new ArgumentOutOfRangeException("value is too short");
            }

            _value = PIZZAIntHelper.GetNBytes(value, 3);
        }

        public PIZZAInt3()
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
                _value = PIZZAIntHelper.GetBytesFromLong((long)value, 3);
            }
        }

        public byte[] GetBytes()
        {
            return _value;
        }

        public static PIZZAInt3 FromBytes(byte[] value)
        {
            return new PIZZAInt3(value);
        }
    }
}
