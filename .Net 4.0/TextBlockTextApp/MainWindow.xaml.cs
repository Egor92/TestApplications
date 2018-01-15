using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TextBlockTextApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MyWindow_Loaded;
        }

        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MyWindow_Loaded;
            var text = GetFullText(tb);
            MessageBox.Show(text);
        }

        private static string GetFullText(TextBlock textBlock)
        {
            return textBlock.Inlines.Select(GetFullText)
                            .Aggregate(string.Empty, (s, s1) => s + s1);
        }

        private static string GetFullText(Inline inline)
        {
            var run = inline as Run;
            if (run != null)
            {
                return run.Text;
            }

            var span = inline as Span;
            if (span != null)
            {
                return span.Inlines.Select(GetFullText)
                           .Aggregate(string.Empty, (s, s1) => s + s1);
            }

            return string.Empty;
        }
    }
}