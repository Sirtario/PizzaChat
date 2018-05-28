using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIZZA.Core
{
    public class PIZZAString
    {
        private byte[] _value = new byte[0];

        public PIZZAInt Length { get; }

        private PIZZAString (byte[] value)
        {
            var valueList = value.ToList();

            valueList.RemoveRange(0, 2);

            Value = Encoding.UTF8.GetString(valueList.ToArray());
        }

        public PIZZAString()
        {
            Length = new PIZZAInt();
        }

        public string Value
        {
            get
            {
                return Encoding.UTF8.GetString(_value);
            }
            set
            {
                _value = Encoding.UTF8.GetBytes(value);
                Length.Value = value.Length;
            }
        }

        public PIZZAString FromBytes(byte[] value)
        {
            var result = new PIZZAString(value);

            return result;
        }
    }
}
