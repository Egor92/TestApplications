using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SetterValueForDependencyPropertyInTrigger
{
    class SuperPuperControl : Control
    {
        public static readonly DependencyProperty BrushPropertyProperty = 
            DependencyProperty.Register("BrushProperty", typeof (Brush), typeof (SuperPuperControl), new PropertyMetadata(Brushes.Red));

        public Brush BrushProperty
        {
            get { return (Brush) GetValue(BrushPropertyProperty); }
            set { SetValue(BrushPropertyProperty, value); }
        }

        public static readonly DependencyProperty BoolPropertyProperty = 
            DependencyProperty.Register("BoolProperty", typeof (bool), typeof (SuperPuperControl), new PropertyMetadata(false));

        public bool BoolProperty
        {
            get { return (bool) GetValue(BoolPropertyProperty); }
            set { SetValue(BoolPropertyProperty, value); }
        }

        static SuperPuperControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (SuperPuperControl), new FrameworkPropertyMetadata(typeof (SuperPuperControl)));
        }
    }
}
