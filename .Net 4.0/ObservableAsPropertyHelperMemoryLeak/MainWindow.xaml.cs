using System.Windows;

namespace ObservableAsPropertyHelperMemoryLeak
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
        }

        public MainWindowViewModel ViewModel
        {
            get { return (MainWindowViewModel)DataContext; }
            private set { DataContext = value; }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Start();
        }
    }
}
