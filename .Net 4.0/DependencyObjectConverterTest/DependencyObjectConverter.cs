using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace DependencyObjectConverterTest
{
    public class DependencyObjectConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty MultProperty =
            DependencyProperty.Register("Mult", typeof (double), typeof (DependencyObjectConverter), new PropertyMetadata(default(double)));

        public double Mult
        {
            get { return (double) GetValue(MultProperty); }
            set { SetValue(MultProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * Mult;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
