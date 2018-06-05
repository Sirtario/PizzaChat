using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Core
{
    internal static class PIZZAIntHelper
    {
        internal static byte[] GetNBytes(byte[] value, int n)
        {
            var result = new byte[n];

            for (int i = 0; i < n; i++)
            {
                result[i] = value[i];
            }

            return result;
        }

        internal static long GetValueFromBytes(byte[] bytes)
        {
            var result = 0L;
       
            for (int i = 0; i < bytes.Length; i++)
            {
                result += bytes[bytes.Length - i - 1] * Convert.ToInt64(Math.Pow(Convert.ToDouble(256), Convert.ToDouble(i)));
            }

            return result;
        }

        internal static byte[] GetBytesFromLong(long value, int bytes)
        {
            var result = new byte[bytes];

            for (int i = 0; i < bytes; i++)
            {
                result[bytes - i - 1] = (byte)(value >> i * 8);
            }

            return result;
        }
    }
}
