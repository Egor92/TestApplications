using System.Windows;
using SharpAvi;
using SharpAvi.Codecs;
using SharpAvi.Output;

namespace CaptureVideoApp
{
    public class RecorderParams
    {
        public RecorderParams(string filename, int frameRate, FourCC encoder, int quality)
        {
            _fileName = filename;
            FramesPerSecond = frameRate;
            _codec = encoder;
            Quality = quality;

            Height = (int)SystemParameters.PrimaryScreenHeight;
            Width = (int)SystemParameters.PrimaryScreenWidth;
        }

        private readonly string _fileName;
        public readonly int FramesPerSecond;
        public readonly int Quality;
        private readonly FourCC _codec;

        public int Height { get; private set; }
        public int Width { get; private set; }

        public AviWriter CreateAviWriter()
        {
            return new AviWriter(_fileName)
            {
                FramesPerSecond = FramesPerSecond,
                EmitIndex1 = true,
            };
        }

        public IAviVideoStream CreateVideoStream(AviWriter writer)
        {
            // Select encoder type based on FOURCC of codec
            if (_codec == KnownFourCCs.Codecs.Uncompressed)
                return writer.AddUncompressedVideoStream(Width, Height);

            if (_codec == KnownFourCCs.Codecs.MotionJpeg)
                return writer.AddMotionJpegVideoStream(Width, Height, Quality);

            return writer.AddMpeg4VideoStream(Width, Height, (double)writer.FramesPerSecond,
                                              // It seems that all tested MPEG-4 VfW codecs ignore the quality affecting parameters passed through VfW API
                                              // They only respect the settings from their own configuration dialogs, and Mpeg4VideoEncoder currently has no support for this
                                              quality: Quality,
                                              codec: _codec,
                                              // Most of VfW codecs expect single-threaded use, so we wrap this encoder to special wrapper
                                              // Thus all calls to the encoder (including its instantiation) will be invoked on a single thread although encoding (and writing) is performed asynchronously
                                              forceSingleThreadedAccess: true);
        }
    }
}