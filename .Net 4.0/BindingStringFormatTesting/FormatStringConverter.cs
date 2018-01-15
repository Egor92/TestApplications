using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace BindingStringFormatTesting
{
    public class FormatStringConverter : MarkupExtension, IValueConverter
    {
        public string Format { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (string) value;

            if (Format == null)
                return str;

            return string.Format(Format, str);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
