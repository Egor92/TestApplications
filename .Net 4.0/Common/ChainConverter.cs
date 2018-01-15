using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Common
{
    [ContentProperty("Converter")]
    public abstract class ChainConverter<TFrom, TTo> : MarkupExtension, IChainConverter
    {
        public IValueConverter Converter { get; set; }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Converter != null)
                value = Converter.Convert(value, targetType, parameter, culture);
            ChainConverterHelper.CheckValueForType<TFrom>(value);
            return Convert((TFrom)value, targetType, parameter, culture);
        }

        protected abstract TTo Convert(TFrom value, Type targetType, object parameter, CultureInfo culture);

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Converter != null)
                value = Converter.ConvertBack(value, targetType, parameter, culture);
            ChainConverterHelper.CheckValueForType<TTo>(value);
            return ConvertBack((TTo)value, targetType, parameter, culture);
        }

        protected abstract TFrom ConvertBack(TTo value, Type targetType, object parameter, CultureInfo culture);

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
