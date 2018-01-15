using System;
using System.Windows;
using System.Windows.Controls;

namespace CoerceValueTesting
{
    public class RangeControl : Control
    {
        static RangeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (RangeControl), new FrameworkPropertyMetadata(typeof (RangeControl)));
        }

        #region Min

        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register("Min", typeof (double), typeof (RangeControl), new PropertyMetadata(default(double), OnMinChanged, CoerceMin), ValidateMin);

        public double Min
        {
            get { return (double)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        private static void OnMinChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var rangeControl = (RangeControl)sender;
            rangeControl.CoerceValue(ValueProperty);
            rangeControl.CoerceValue(MaxProperty);
        }

        private static object CoerceMin(DependencyObject sender, object baseValue)
        {
            var rangeControl = (RangeControl)sender;
            var value = (double)baseValue;
            value = Math.Min(value, rangeControl.Max);
            return value;
        }

        private static bool ValidateMin(object value)
        {
            return ValidateValue((double)value);
        }

        #endregion

        #region Max

        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(double), typeof(RangeControl), new PropertyMetadata(default(double), OnMaxChanged, CoerceMax), ValidateMax);

        public double Max
        {
            get { return (double)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        private static void OnMaxChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var rangeControl = (RangeControl)sender;
            rangeControl.CoerceValue(ValueProperty);
            rangeControl.CoerceValue(MinProperty);
        }

        private static object CoerceMax(DependencyObject sender, object baseValue)
        {
            var rangeControl = (RangeControl)sender;
            var value = (double)baseValue;
            value = Math.Max(rangeControl.Min, value);
            return value;
        }

        private static bool ValidateMax(object value)
        {
            return ValidateValue((double)value);
        }

        #endregion

        #region Value

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(RangeControl), new PropertyMetadata(default(double), OnValueChanged, CoerceValue), ValidateValue);

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var rangeControl = (RangeControl)sender;
        }

        private static object CoerceValue(DependencyObject sender, object baseValue)
        {
            var rangeControl = (RangeControl)sender;
            var value = (double)baseValue;
            value = Math.Min(value, rangeControl.Max);
            value = Math.Max(rangeControl.Min, value);
            return value;
        }

        private static bool ValidateValue(object value)
        {
            return ValidateValue((double)value);
        }

        #endregion

        private static bool ValidateValue(double value)
        {
            return !double.IsNaN(value) && !double.IsInfinity(value);
        }
    }
}