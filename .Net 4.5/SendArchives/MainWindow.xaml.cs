using System.Windows;
using System.Windows.Controls;

namespace SendArchives
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PasswordBox_OnPasswordChanged1(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel) DataContext).SenderPassword = ((PasswordBox) sender).Password;
        }

        private void PasswordBox_OnPasswordChanged2(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).SenderPasswordRepeat = ((PasswordBox)sender).Password;
        }
    }
}
