using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace MultiBindingTest
{
    public class SetConverterProperiesRequestEventArgs : EventArgs
    {
        internal IValueConverter Converter { get; private set; }
        internal object[] PropertyValues { get; private set; }

        internal SetConverterProperiesRequestEventArgs(IValueConverter converter, object[] propertyValues)
        {
            if (converter == null)
                throw new ArgumentNullException("converter");
            if (propertyValues == null)
                throw new ArgumentNullException("propertyValues");
            Converter = converter;
            PropertyValues = propertyValues;
        }
    }

    [ContentProperty("Converter")]
    internal class MultiValueConverterAdapter : IMultiValueConverter
    {
        private object _LastParameter;

        internal IValueConverter Converter { get; set; }

        internal string StringFormat { get; set; }

        internal bool[] AreValuesSetted { get; set; }

        #region SetConverterProperiesRequest

        public event EventHandler<SetConverterProperiesRequestEventArgs> SetConverterProperiesRequest;

        private void RaiseSetConverterProperiesRequest(IValueConverter converter, object[] propertyValues)
        {
            var handler = SetConverterProperiesRequest;
            if (handler != null)
                handler(this, new SetConverterProperiesRequestEventArgs(converter, propertyValues));
        }

        #endregion

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var result = values[0];
            var index = 1;

            if (AreValuesSetted[1])
            {
                _LastParameter = values[index];
                index++;
            }

            if (AreValuesSetted[2])
            {
                StringFormat = values[index] as string;
                index++;
            }

            if (Converter != null)
            {
                var propertyValues = GetPropertyValues(values);
                RaiseSetConverterProperiesRequest(Converter, propertyValues);
                result = Converter.Convert(values[0], targetType, _LastParameter, culture);
            }

            if (result == null)
                return null;

            if (!string.IsNullOrEmpty(StringFormat))
            {
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

        private object[] GetPropertyValues(object[] values)
        {
            const int serviceBindingsCount = ConverterBinding.ServiceBindingsCount;
            var valuesStartIndex = AreValuesSetted.Take(serviceBindingsCount).Count(x => x);
            var valueIndex = valuesStartIndex;
            var propertyValues = new object[AreValuesSetted.Length - serviceBindingsCount];
            for (int i = serviceBindingsCount; i < AreValuesSetted.Length; i++)
            {
                var isValueSetted = AreValuesSetted[i];
                var propertyIndex = i - serviceBindingsCount;
                if (isValueSetted)
                {
                    propertyValues[propertyIndex] = values[valueIndex];
                    valueIndex++;
                }
                else
                {
                    propertyValues[propertyIndex] = DependencyProperty.UnsetValue;
                }
            }
            return propertyValues;
        }
    }

    public class ConverterBinding : MarkupExtension
    {
        internal const int ServiceBindingsCount = 3;

        public BindingBase Binding { get; set; }

        public IValueConverter Converter { get; set; }

        public BindingBase ConverterParameterBinding { get; set; }

        public string StringFormat { get; set; }

        public BindingBase StringFormatBinding { get; set; }

        public object TargetNullValue { get; set; }

        public BindingMode Mode { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var multiBinding = new MultiBinding { TargetNullValue = TargetNullValue };

            var adapter = new MultiValueConverterAdapter
            {
                Converter = Converter,
                StringFormat = StringFormat,
            };

            var converterPropertiesBindings = GetConverterPropertiesBindings();
            var valuesCount = converterPropertiesBindings.Count() + ServiceBindingsCount;
            bool[] areValuesSetted = new bool[valuesCount];

            if (Binding == null)
                return null;
            areValuesSetted[0] = true;

            multiBinding.Bindings.Add(Binding);
            if (ConverterParameterBinding != null)
            {
                multiBinding.Bindings.Add(ConverterParameterBinding);
                areValuesSetted[1] = true;
            }
            if (StringFormatBinding != null)
            {
                multiBinding.Bindings.Add(StringFormatBinding);
                areValuesSetted[2] = true;
            }

            int index = ServiceBindingsCount;
            foreach (var propertyBinding in converterPropertiesBindings)
            {
                if (propertyBinding != null)
                {
                    areValuesSetted[index] = true;
                    multiBinding.Bindings.Add(propertyBinding);
                }
                index++;
            }

            adapter.AreValuesSetted = areValuesSetted;
            adapter.SetConverterProperiesRequest += Adapter_SetConverterProperiesRequest;

            multiBinding.Converter = adapter;
            multiBinding.Mode = Mode;

            return multiBinding.ProvideValue(serviceProvider);
        }

        protected virtual IEnumerable<BindingBase> GetConverterPropertiesBindings()
        {
            yield break;
        }

        protected virtual void SetConverterProperies(IValueConverter converter, object[] propertyValues)
        {

        }

        private void Adapter_SetConverterProperiesRequest(object sender, SetConverterProperiesRequestEventArgs e)
        {
            SetConverterProperies(e.Converter, e.PropertyValues);
        }
    }

    public static class ConverterBindingHelper
    {
        public static void SetPropertyValue<TConverter, TValue>(IValueConverter converter, object propertyValue, Action<TConverter, TValue> setValueAction)
            where TConverter : class
        {
            var valueConverter = converter as TConverter;
            if (valueConverter == null)
                return;

            if (propertyValue != DependencyProperty.UnsetValue && propertyValue is TValue)
            {
                setValueAction(valueConverter, (TValue)propertyValue);
            }
        }
    }
}
