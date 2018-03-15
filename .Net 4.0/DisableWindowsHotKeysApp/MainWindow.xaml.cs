using System.Diagnostics;
using System.Windows.Input;

namespace DisableWindowsHotKeysApp
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            PreviewKeyDown += MainWindow_PreviewKeyDown;
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("Modifiers={0}; Key={1}; KeyStates={2}; SystemKey = {3}", Keyboard.Modifiers, e.Key, e.KeyStates, e.SystemKey);

            if (Keyboard.Modifiers == ModifierKeys.Alt && e.SystemKey == Key.Space)
            {
                e.Handled = true;
            }
            else
            {
                base.OnKeyDown(e);
            }
        }
    }
}