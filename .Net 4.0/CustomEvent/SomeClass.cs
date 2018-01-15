using System.Windows;
using System.Windows.Controls;

namespace CustomEvent
{
    public class SampleClass : Button
    {
        public static readonly RoutedEvent SampleRoutedEvent =
            EventManager.RegisterRoutedEvent("SampleEvent", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (SampleClass));

        public event RoutedEventHandler SampleEvent
        {
            add { AddHandler(SampleRoutedEvent, value); }
            remove { RemoveHandler(SampleRoutedEvent, value); }
        }
    }
}