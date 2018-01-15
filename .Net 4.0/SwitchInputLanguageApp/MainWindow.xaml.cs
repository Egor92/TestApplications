using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace SwitchInputLanguageApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InputLanguageManager.Current.InputLanguageChanged += Current_InputLanguageChanged;
        }

        private void Current_InputLanguageChanged(object sender, InputLanguageEventArgs e)
        {
            LanguagesListView.SelectedItem = InputLanguageManager.Current.CurrentInputLanguage;
        }

        private void MyComboBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            LanguagesListView.ItemsSource = InputLanguageManager.Current.AvailableInputLanguages;
            LanguagesListView.SelectedItem = InputLanguageManager.Current.CurrentInputLanguage;
        }

        private void MyComboBox_OnSelected(object sender, RoutedEventArgs e)
        {
            InputLanguageManager.Current.CurrentInputLanguage = (CultureInfo)LanguagesListView.SelectedItem;
        }

        private void SwitchButton_OnLostFocus(object sender, RoutedEventArgs e)
        {
            SwitchButton.IsChecked = false;
        }
    }
}
