using System;

namespace PIZZA.Core
{
    public class PIZZAInt5
    {
        private byte[] _value = new byte[5];

        private PIZZAInt5(byte[] value)
        {
            if (value.Length < 5)
            {
                throw new ArgumentOutOfRangeException("value is too short");
            }

            _value = PIZZAIntHelper.GetNBytes(value, 5);
        }

        public PIZZAInt5()
        {
            Value = 0;
        }

        public long Value
        {
            get
            {
                return PIZZAIntHelper.GetValueFromBytes(_value);
            }
            set
            {
                _value = PIZZAIntHelper.GetBytesFromLong(value, 5);
            }
        }

        public byte[] GetBytes()
        {
            return _value;
        }

        public static PIZZAInt5 FromBytes(byte[] value)
        {
            return new PIZZAInt5(value);
        }
    }
}
