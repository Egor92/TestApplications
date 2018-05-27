using System;
using System.IO;
using System.Text;
using System.Windows;

namespace SendArchives
{
    public partial class App : Application
    {
        #region Overrides of Application

        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            var mainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
            mainWindow.Show();
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string timeString = DateTime.Now.ToString(@"yyyy-MM-dd HH-mm-ss");
            string path = $@"{Directory.GetCurrentDirectory()}\crash reports\crash report ({timeString}).txt";

            StringBuilder contentBuilder = new StringBuilder();

            var exception = (Exception) e.ExceptionObject;

            while (exception != null)
            {
                contentBuilder.AppendLine(exception.Message);
                contentBuilder.AppendLine();
                contentBuilder.AppendLine(exception.StackTrace);
                contentBuilder.AppendLine();
                contentBuilder.AppendLine("========================================");
                contentBuilder.AppendLine();

                exception = exception.InnerException;
            }

            File.WriteAllText(path, contentBuilder.ToString());
            string message = $@"Произошла необработанная ошибка. Приложение будет закрыто. Файл с логами находится по пути: ""{path}""";
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion
    }
}
