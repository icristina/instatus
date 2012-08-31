using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using Instatus.Core;

namespace Instatus.Integration.Server
{
    public class WpfImaging : IImaging
    {
        public void Crop(Stream inputStream, Stream outputStream, int left, int top, int width, int height)
        {
            var rectangle = new Int32Rect(left, top, width, height);
            var bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();
            bitmapImage.SourceRect = rectangle;
            bitmapImage.StreamSource = inputStream;
            bitmapImage.EndInit();

            WriteToOutputStream(bitmapImage, outputStream);
        }

        public void Resize(Stream inputStream, Stream outputStream, int width, int height)
        {
            var bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();
            bitmapImage.DecodePixelWidth = width;
            bitmapImage.DecodePixelHeight = height;
            bitmapImage.StreamSource = inputStream;
            bitmapImage.EndInit();

            WriteToOutputStream(bitmapImage, outputStream);
        }

        public void Cover(Stream inputStream, Stream outputStream, int width, int height)
        {
            throw new NotImplementedException();
        }

        public void Contain(Stream inputStream, Stream outputStream, int width, int height)
        {
            throw new NotImplementedException();
        }
        
        public void WriteToOutputStream(BitmapSource bitmapSource, Stream outputStream)
        {
            var encoder = new JpegBitmapEncoder();
            var bitmapFrame = BitmapFrame.Create(bitmapSource);

            encoder.QualityLevel = 80;
            encoder.Frames.Add(bitmapFrame);
            encoder.Save(outputStream);
        }
    }
}
