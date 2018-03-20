using System;
using System.Windows;

namespace CaptureVideoApp
{
    public partial class MainWindow
    {
        private Recorder _rec;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            // Using MotionJpeg as Avi encoder,
            // output to 'out.avi' at 10 Frames per second, 70% quality
            var fileName = string.Format("{0}.avi", DateTime.Now.ToString("yyyy-MM-dd hh-mm"));

            var recorderParams = new RecorderParams(fileName, 2, SharpAvi.KnownFourCCs.Codecs.MotionJpeg, 70);
            _rec = new Recorder(recorderParams);
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            // Finish Writing
            _rec.Dispose();
        }
    }
}
