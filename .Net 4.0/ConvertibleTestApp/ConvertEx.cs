using System;
using System.Threading;

namespace ConvertibleTestApp
{
    public static class ConvertEx
    {
        public static bool TryConvert<T>(object value, IFormatProvider formatProvider, out T result)
        {
            Type targetType = typeof(T);

            if (value is T)
            {
                result = (T)value;
                return true;
            }

            if (value == null && !targetType.IsValueType)
            {
                result = (T)value;
                return true;
            }

            var nullableUnderlyingType = Nullable.GetUnderlyingType(targetType);

            if (value == null && nullableUnderlyingType != null)
            {
                result = (T)value;
                return true;
            }

            if (nullableUnderlyingType != null)
                targetType = nullableUnderlyingType;

            if (value == null && targetType.IsValueType)
            {
                result = default(T);
                return false;
            }

            var valueTypeCode = System.Convert.GetTypeCode(value);
            if (valueTypeCode == TypeCode.Object || valueTypeCode == TypeCode.Empty)
            {
                result = default(T);
                return false;
            }
            TypeCode targetTypeCode;
            if (!Enum.TryParse(targetType.Name, out targetTypeCode))
            {
                result = default(T);
                return false;
            }
            result = (T)System.Convert.ChangeType(value, targetTypeCode, formatProvider);
            return true;
        }

        public static bool TryConvert<T>(object value, out T result)
        {
            return TryConvert(value, Thread.CurrentThread.CurrentCulture, out result);
        }

        public static T Convert<T>(object value, IFormatProvider formatProvider)
        {
            T result;
            if (!TryConvert<T>(value, formatProvider, out result))
                throw new Exception(string.Format("Can not convert {0} to type '{1}'", GetValueTypeString(value), typeof(T)));
            return result;
        }

        public static T Convert<T>(object value)
        {
            return Convert<T>(value, Thread.CurrentThread.CurrentCulture);
        }

        private static string GetValueTypeString(object value)
        {
            if (value == null)
                return "'null'";
            return string.Format("value of type '{0}'", value.GetType());
        }
    }
}
