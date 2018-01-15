using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MultiBindingTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindowViewModel _viewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            _viewModel = new MainWindowViewModel();
            new MainWindow()
                {
                    DataContext = _viewModel
                }.Show();
            new Thread(Change).Start();
        }

        private void Change()
        {
            while (true)
            {
                Dispatcher.Invoke((Action)(() =>
                {
                    _viewModel.Text = RandomStringHelper.GetWord(5, 10, WordCase.Mixed);
                    _viewModel.Value = RandomHelper.GetInt(-1, 1);
                }));
                Thread.Sleep(TimeSpan.FromMilliseconds(2000));
            }
        }
    }
}
