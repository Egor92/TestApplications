using System;
using System.IO;
using System.Net;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace PopupParentApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            WebRequest req = WebRequest.Create("http://project-megaroks931128.codeanyapp.com/news/add/?usersid=76561198049827777");
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string Out = sr.ReadToEnd();
            sr.Close();
            var obj = JObject.Parse(Out);
            var id = Convert.ToInt32(obj["steamid"]);
        }
    }
}
