using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace BindingStringFormatTesting
{
    [ContentProperty("Converter")]
    internal class MultiValueConverterAdapter : IMultiValueConverter
    {
        private object _LastParameter;

        internal IValueConverter Converter { get; set; }

        internal string StringFormat { get; set; }

        internal bool IsParameterBindingSetted { get; set; }

        internal bool IsStringFormatBindingSetted { get; set; }

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var result = values[0];
            var index = 1;

            if (IsParameterBindingSetted)
            {
                _LastParameter = values[index];
                index++;
            }

            if (IsStringFormatBindingSetted)
            {
                StringFormat = values[index] as string;
                index++;
            }

            if (Converter != null)
            {
                result = Converter.Convert(values[0], targetType, _LastParameter, culture);
            }

            if (result == null)
                return null;

            if (!string.IsNullOrEmpty(StringFormat))
            {
                //string format = string.Format("{{0:{0}}}", StringFormat);
                result = string.Format(StringFormat, result);
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (Converter == null) return new[] { value }; // Required for VS design-time

            return new[] { Converter.ConvertBack(value, targetTypes[0], _LastParameter, culture) };
        }

        #endregion
    }

    public class ConverterBinding : MarkupExtension
    {
        public BindingBase Binding { get; set; }

        public IValueConverter Converter { get; set; }

        public BindingBase ConverterParameterBinding { get; set; }

        public string StringFormat { get; set; }

        public BindingBase StringFormatBinding { get; set; }

        public object TargetNullValue { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var multiBinding = new MultiBinding { TargetNullValue = TargetNullValue };

            var adapter = new MultiValueConverterAdapter
            {
                Converter = Converter,
                StringFormat = StringFormat,
            };

            if (Binding == null)
                return null;
            multiBinding.Bindings.Add(Binding);
            if (ConverterParameterBinding != null)
            {
                multiBinding.Bindings.Add(ConverterParameterBinding);
                adapter.IsParameterBindingSetted = true;
            }
            if (StringFormatBinding != null)
            {
                multiBinding.Bindings.Add(StringFormatBinding);
                adapter.IsStringFormatBindingSetted = true;
            }

            multiBinding.Converter = adapter;

            return multiBinding.ProvideValue(serviceProvider);
        }
    }
}
