using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace MultiBindingTest
{
    public class TextMultiConverter : MarkupExtension, IMultiValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var value = (int)values[0];
            var text  = (string)values[1];
            if (value < 0)
                return value.ToString();
            if (value > 0)
                return text;
            return "null";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
