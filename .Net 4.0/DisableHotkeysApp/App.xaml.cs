using System.Windows;
using DisableHotkeysApp.Models;
using DisableHotkeysApp.ViewModels;
using DisableHotkeysApp.Views;

namespace DisableHotkeysApp
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new Window
            {
                Title = "Hotkeys manager",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SizeToContent = SizeToContent.WidthAndHeight
            };

            var hotkeysManager = new HotkeysManager();
            var mainVM = new MainViewModel(hotkeysManager);
            window.Content = new MainView
            {
                DataContext = mainVM,
            };
            window.Show();
        }
    }
}