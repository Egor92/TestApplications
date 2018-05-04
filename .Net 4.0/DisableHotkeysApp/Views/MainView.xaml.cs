using System.Windows;

namespace DisableHotkeysApp.Views
{
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void OnExitButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}