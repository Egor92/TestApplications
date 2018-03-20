using System.Windows;
using SharpAvi;

namespace CaptureVideoApp
{
    public class RecorderParams
    {
        public RecorderParams(string filename, int frameRate, FourCC encoder, int quality)
        {
            FileName = filename;
            FramesPerSecond = frameRate;
            Codec = encoder;
            Quality = quality;

            Height = (int)SystemParameters.PrimaryScreenHeight;
            Width = (int)SystemParameters.PrimaryScreenWidth;
        }

        public string FileName { get; private set; }
        public int FramesPerSecond { get; private set; }
        public int Quality { get; private set; }
        public FourCC Codec { get; private set; }

        public int Height { get; private set; }
        public int Width { get; private set; }
    }
}