using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ReactiveUI;

namespace ReactiveCollectionItemRemoved
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var list = Enumerable.Range(0, 5000).ToList();
            var reactiveCollection = new ReactiveCollection<int>(list)
            {
                ChangeTrackingEnabled = true,
            };
            reactiveCollection.ResetChangeThreshold = 1.0;
            reactiveCollection.ItemsRemoved.Subscribe(x => Debug.WriteLine(x));
            reactiveCollection.Changed.Subscribe(x => Debug.WriteLine(x));
            var itemsToRemove = Enumerable.Range(0, 5000).ToList();
            reactiveCollection.RemoveAll(itemsToRemove);
        }
    }
}
