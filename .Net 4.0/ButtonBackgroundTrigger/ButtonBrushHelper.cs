using System.Windows;
using System.Windows.Media;

namespace ButtonBackgroundTrigger
{
    public static class ButtonBrushHelper
    {
        #region MouseOverBrush

        public static readonly DependencyProperty MouseOverBrushProperty =
            DependencyProperty.RegisterAttached("MouseOverBrush", typeof (Brush), typeof (ButtonBrushHelper));

        public static void SetMouseOverBrush(UIElement element, Brush value)
        {
            element.SetValue(MouseOverBrushProperty, value);
        }

        public static Brush GetMouseOverBrush(UIElement element)
        {
            return (Brush)element.GetValue(MouseOverBrushProperty);
        }

        #endregion

        #region MouseLeaveBrush

        public static readonly DependencyProperty MouseLeaveBrushProperty =
            DependencyProperty.RegisterAttached("MouseLeaveBrush", typeof (Brush), typeof (ButtonBrushHelper));

        public static void SetMouseLeaveBrush(UIElement element, Brush value)
        {
            element.SetValue(MouseLeaveBrushProperty, value);
        }

        public static Brush GetMouseLeaveBrush(UIElement element)
        {
            return (Brush)element.GetValue(MouseLeaveBrushProperty);
        }

        #endregion
    }
}
