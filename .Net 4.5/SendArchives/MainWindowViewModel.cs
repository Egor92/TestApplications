using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using SendArchives.Models;

namespace SendArchives
{
    public class MainWindowViewModel : NotificationObject
    {
        #region Fields

        private readonly CredentialsSender _credentialsSender = new CredentialsSender();

        #endregion

        #region Ctor

        public MainWindowViewModel()
        {
            Host = "smtp.gmail.com";
            Port = 587;
            Timeout = 4000;
            DeliveryMethod = SmtpDeliveryMethod.Network;
            EnableSsl = true;
        }

        #endregion

        #region Properties

        #region IsBusy

        private bool _isEnabled = true;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged(() => IsEnabled);
                RaisePropertyChanged(() => Title);
            }
        }

        #endregion

        #region Title

        public string Title
        {
            get { return $"Credentials sender {(IsEnabled ? null : "[Sending....]")}"; }
        }

        #endregion

        #region Host

        public string Host { get; set; }

        #endregion

        #region Port

        public int Port { get; set; }

        #endregion

        #region Timeout

        public int Timeout { get; set; }

        #endregion

        #region SmtpDeliveryMethod

        public SmtpDeliveryMethod DeliveryMethod { get; set; }

        #endregion

        #region EnableSsl

        public bool EnableSsl { get; set; }

        #endregion

        #region SenderEmail

        public string SenderEmail { get; set; }

        #endregion

        #region SenderPassword

        public string SenderPassword { get; set; }

        #endregion

        #region SenderPasswordRepeat

        public string SenderPasswordRepeat { get; set; }

        #endregion

        #region InputDataFilePath

        private string _inputDataFilePath;

        public string InputDataFilePath
        {
            get { return _inputDataFilePath; }
            set
            {
                _inputDataFilePath = value;
                RaisePropertyChanged(() => InputDataFilePath);
            }
        }

        #endregion

        #endregion

        #region SelectInpitFileCommand

        private ICommand _selectInpitFileCommand;

        public ICommand SelectInpitFileCommand
        {
            get { return _selectInpitFileCommand ?? (_selectInpitFileCommand = new DelegateCommand(SelectInpitFile)); }
        }

        private void SelectInpitFile()
        {
            var openFileDialog = new OpenFileDialog()
            {
                Title = "Select file",
                Filter = "txt files|*.txt"
            };
            if (openFileDialog.ShowDialog() != true)
                return;

            InputDataFilePath = openFileDialog.FileName;
        }

        #endregion

        #region SendCommand

        private ICommand _sendCommand;

        public ICommand SendCommand
        {
            get { return _sendCommand ?? (_sendCommand = new DelegateCommand(Send)); }
        }

        private void Send()
        {
            if (string.IsNullOrWhiteSpace(Host))
            {
                MessageBox.Show("Host must be set");
                return;
            }

            if (Port < 0)
            {
                MessageBox.Show("Port must be positive");
                return;
            }

            if (Timeout < 0)
            {
                MessageBox.Show("Timeout must be positive");
                return;
            }

            if (string.IsNullOrWhiteSpace(SenderEmail))
            {
                MessageBox.Show("Sender e-mail must be set");
                return;
            }

            if (string.IsNullOrWhiteSpace(SenderPassword))
            {
                MessageBox.Show("Sender password must be set");
                return;
            }

            if (!string.Equals(SenderPassword, SenderPasswordRepeat))
            {
                MessageBox.Show("Passwords aren't equals");
                return;
            }

            if (string.IsNullOrWhiteSpace(InputDataFilePath))
            {
                MessageBox.Show("Input data file must be set");
                return;
            }

            IsEnabled = false;
            Task.Run(() =>
            {
                try
                {
                    var emailProperties = new EmailProperties
                    {
                        Host = Host,
                        Port = Port,
                        Timeout = Timeout,
                        DeliveryMethod = DeliveryMethod,
                        EnableSsl = EnableSsl,
                        SenderEmail = SenderEmail,
                        SenderPassword = SenderPassword,
                    };
                    var result = _credentialsSender.Send(InputDataFilePath, emailProperties);
                    var aggregatedResult = result.Data.IsSendingSuccessfulByLogin.Select(x => string.Format($"{x.Key}: sending result - {x.Value}"))
                                                 .Aggregate(string.Empty, (s1, s2) => $"{s1}\n{s2}");

                    aggregatedResult = $"{result.ErrorMessage}\n{aggregatedResult}";
                    var reportsDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
                    Directory.CreateDirectory(reportsDirectoryPath);
                    string timeString = DateTime.Now.ToString(@"yyyy-MM-dd HH-mm-ss");
                    var reportFilePath = $"Reports\\{timeString}.txt";
                    File.WriteAllText(reportFilePath, aggregatedResult);

                    if (result.IsSuccess)
                    {
                        MessageBox.Show($"Sending has finished SUCCESSFULLY\nSee report file '{reportFilePath}'");
                    }
                    else
                    {
                        MessageBox.Show($"Sending has finished WITH FAILS\nSee report file '{reportFilePath}' for additional details");
                    }
                }
                finally
                {
                    IsEnabled = true;
                }
            });
        }

        #endregion
    }
}
