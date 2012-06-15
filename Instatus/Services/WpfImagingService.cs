using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Instatus.Models;

namespace Instatus.Services
{
    public class WpfImagingService : IImagingService
    {
        public Stream Crop(Stream stream, Element area)
        {
            var bitmapSource = stream.ConvertToBitmapSource();
            var croppedImage = new CroppedBitmap(bitmapSource, area.ConvertToInt32Rect());

            return croppedImage.ConvertToJpgStream();
        }

        public Stream Transform(Stream stream, Models.Transform transform)
        {
            throw new NotImplementedException();
        }
    }

    public static class WpfImagingExtensions
    {
        public static BitmapSource ConvertToBitmapSource(this Stream stream)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            return bitmapImage;
        }

        public static Int32Rect ConvertToInt32Rect(this Element element)
        {
            return new Int32Rect(element.Left, element.Top, element.Width, element.Height);
        }

        public static Stream ConvertToJpgStream(this BitmapSource bitmapSource)
        {
            var memoryStream = new MemoryStream();
            var encoder = new JpegBitmapEncoder();

            encoder.QualityLevel = 80;
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(memoryStream);

            return memoryStream;
        }
    }
}