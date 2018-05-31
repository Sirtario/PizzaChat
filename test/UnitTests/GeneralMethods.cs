﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    static class GeneralMethods
    {
        public static bool CompareMenberwise(this byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i]!=b[i])
                {
                    return false;
                }
            }

            return true;
        }

    }
}
