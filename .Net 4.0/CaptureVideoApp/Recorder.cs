using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using SharpAvi.Output;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace CaptureVideoApp
{
    // Used to Configure the Recorder

    public class Recorder : IDisposable
    {
        #region Fields

        private AviWriter _writer;
        private readonly RecorderParams _recorderParams;
        private IAviVideoStream _videoStream;
        private readonly Thread _screenThread;
        private readonly ManualResetEvent _stopThread = new ManualResetEvent(false);

        #endregion

        public Recorder(RecorderParams recorderParams)
        {
            _recorderParams = recorderParams;

            _screenThread = new Thread(RecordScreen)
            {
                Name = typeof (Recorder).Name + ".RecordScreen",
                IsBackground = true
            };

            _screenThread.Start();
        }

        public void Dispose()
        {
            _stopThread.Set();
            _screenThread.Join();

            // Close writer: the remaining data is written to a file and file is closed
            _writer.Close();

            _stopThread.Dispose();
        }

        private void RecordScreen()
        {

            // Create AVI writer and specify FPS
            _writer = _recorderParams.CreateAviWriter();

            // Create video stream
            _videoStream = _recorderParams.CreateVideoStream(_writer);
            // Set only name. Other properties were when creating stream, 
            // either explicitly by arguments or implicitly by the encoder used
            _videoStream.Name = "Captura";

            var frameInterval = TimeSpan.FromSeconds(1 / (double)_writer.FramesPerSecond);
            var buffer = new byte[_recorderParams.Width*_recorderParams.Height*4];
            Task videoWriteTask = null;
            var timeTillNextFrame = TimeSpan.Zero;

            while (!_stopThread.WaitOne(timeTillNextFrame))
            {
                var timestamp = DateTime.Now;

                Screenshot(buffer);

                // Wait for the previous frame is written
                if (videoWriteTask != null)
                {
                    videoWriteTask.Wait();
                }

                // Start asynchronous (encoding and) writing of the new frame
                _videoStream.WriteFrame(true, buffer, 0, buffer.Length);

                timeTillNextFrame = timestamp + frameInterval - DateTime.Now;
                if (timeTillNextFrame < TimeSpan.Zero)
                    timeTillNextFrame = TimeSpan.Zero;
            }

            // Wait for the last frame is written
            if (videoWriteTask != null)
            {
                videoWriteTask.Wait();
            }
        }

        public void Screenshot(byte[] buffer)
        {
            using (var bmp = new Bitmap(_recorderParams.Width, _recorderParams.Height))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, new Size(_recorderParams.Width, _recorderParams.Height), CopyPixelOperation.SourceCopy);

                    g.Flush();

                    var bits = bmp.LockBits(new Rectangle(0, 0, _recorderParams.Width, _recorderParams.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                    Marshal.Copy(bits.Scan0, buffer, 0, buffer.Length);
                    bmp.UnlockBits(bits);
                }
            }
        }
    }
}