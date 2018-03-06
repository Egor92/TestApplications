using System.Windows;

namespace TabControlTest
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
            window.Show();
        }
    }
}