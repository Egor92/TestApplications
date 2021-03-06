﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncryptionApp.Common
{
    public static class ComparableExtensions
    {
        public static T Limit<T>(this T value, T min, T max)
            where T : IComparable<T>
        {
            if (value.CompareTo(max) > 0)
                return max;
            if (value.CompareTo(min) < 0)
                return min;
            return value;
        }
    }
}
