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
            var inputDimensions = CalculateOriginalDimensions(inputStream);
            var rectangle = CalculateCoverDimensions(inputDimensions, new Tuple<int, int>(width, height));
            var bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();
            bitmapImage.SourceRect = rectangle;
            bitmapImage.DecodePixelWidth = width;
            bitmapImage.DecodePixelHeight = height;
            bitmapImage.StreamSource = inputStream;
            bitmapImage.EndInit();

            WriteToOutputStream(bitmapImage, outputStream);
        }

        public void Contain(Stream inputStream, Stream outputStream, int width, int height)
        {
            var inputDimensions = CalculateOriginalDimensions(inputStream);
            var boundingBoxDimensions = new Tuple<int, int>(width, height);
            var outputDimensions = CalculateResizeDimensions(inputDimensions, boundingBoxDimensions, true, false);

            Resize(inputStream, outputStream, outputDimensions.Item1, outputDimensions.Item2);
        }

        private void ResetStream(Stream stream)
        {
            stream.Position = 0;
        }

        private void WriteToOutputStream(BitmapSource bitmapSource, Stream outputStream)
        {
            var encoder = new JpegBitmapEncoder();
            var bitmapFrame = BitmapFrame.Create(bitmapSource);

            encoder.QualityLevel = 80;
            encoder.Frames.Add(bitmapFrame);
            encoder.Save(outputStream);
        }

        private Tuple<int, int> CalculateOriginalDimensions(Stream inputStream)
        {
            var bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();
            bitmapImage.StreamSource = inputStream;
            bitmapImage.EndInit();

            ResetStream(inputStream);

            return new Tuple<int, int>((int)bitmapImage.Width, (int)bitmapImage.Height);
        }

        private Tuple<int, int> CalculateResizeDimensions(Tuple<int, int> originalDimensions, Tuple<int, int> newDimensions, bool preserveAspectRatio, bool preventEnlarge)
        {
            int originalWidth = originalDimensions.Item1;
            int originalHeight = originalDimensions.Item2;

            int width = newDimensions.Item1;
            int height = newDimensions.Item2;

            if (preserveAspectRatio)
            {
                double num3 = (height * 100.0) / ((double)originalHeight);
                double num4 = (width * 100.0) / ((double)originalWidth);
                if (num3 > num4)
                {
                    height = (int)Math.Round((double)((num4 * originalHeight) / 100.0));
                }
                else if (num3 < num4)
                {
                    width = (int)Math.Round((double)((num3 * originalWidth) / 100.0));
                }
            }

            if (preventEnlarge)
            {
                if (height > originalHeight)
                {
                    height = originalHeight;
                }
                if (width > originalWidth)
                {
                    width = originalWidth;
                }
            }

            return new Tuple<int, int>(width, height);
        }

        public Int32Rect CalculateCoverDimensions(Tuple<int, int> originalDimensions, Tuple<int, int> newDimensions)
        {
            int offset;
            double originalAspectRatio = (double)originalDimensions.Item1 / (double)originalDimensions.Item2;
            double newAspectRatio = (double)newDimensions.Item1 / (double)newDimensions.Item2;

            if (newAspectRatio > originalAspectRatio)
            {
                offset = (int)Math.Round(((double)originalDimensions.Item2 - ((double)originalDimensions.Item1 / newAspectRatio)) / 2.0);
                return new Int32Rect(0, offset, originalDimensions.Item1, originalDimensions.Item2 - (offset * 2));
            }
            else
            {
                offset = (int)Math.Round(((double)originalDimensions.Item1 - ((double)originalDimensions.Item2 * newAspectRatio)) / 2.0);
                return new Int32Rect(offset, 0, originalDimensions.Item1 - (offset * 2), originalDimensions.Item2);
            }
        }
    }
}
