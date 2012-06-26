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

        public void Resize(Stream inputStream, Stream outputStream, int width, int height, bool mask)
        {
            throw new NotImplementedException();
        }

        public void WriteToOutputStream(BitmapSource bitmapSource, Stream outputStream)
        {
            var encoder = new JpegBitmapEncoder();

            encoder.QualityLevel = 80;
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(outputStream);
        }
    }
}
