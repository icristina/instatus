using System;
using Instatus.Models;
using Instatus.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instatus.Tests
{
    [TestClass]
    public class Imaging
    {
        [TestMethod]
        public void Imaging_Crop()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var outputPath = "~/Media/Koala_Cropped.jpg";
            var blobService = new FileSystemBlobService(basePath);
            var imagingService = new WpfImagingService();
            var cropArea = new Element() 
            {
                Left = 100,
                Top = 100,
                Height = 200,
                Width = 200
            };

            using (var originalImage = blobService.Stream("~/Media/Koala.jpg"))
            using (var croppedImage = imagingService.Crop(originalImage, cropArea)) 
            {
                blobService.Save("image/jpg", outputPath, croppedImage);
            }

            using (var outputImage = blobService.LoadImage(outputPath))
            {
                Assert.AreEqual(outputImage.Height, cropArea.Height);
                Assert.AreEqual(outputImage.Width, cropArea.Width);
            }
        }
    }
}
