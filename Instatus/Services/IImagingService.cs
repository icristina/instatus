using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Instatus.Models;

namespace Instatus.Services
{
    public interface IImagingService
    {
        Stream Crop(Stream stream, Element area);
        Stream Transform(Stream stream, Transform transform);
    }
}
