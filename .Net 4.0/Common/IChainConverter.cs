using System;
using System.Windows.Data;

namespace Common
{
    public interface IChainConverter : IValueConverter
    {
        IValueConverter Converter { get; }
    }

    public static class ChainConverterHelper
    {
        public static void CheckValueForType<T>(object value)
        {
            if (value != null && !(value is T))
                throw new ArgumentException(string.Format("value is not of type {0}", typeof(T).FullName), "value");
            if (value == null && typeof(T).IsValueType && !typeof(T).IsNullable())
                throw new Exception(string.Format("value of type {0} cannot be null", typeof(T).FullName));
        }

        private static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }
    }
}
