using Instatus.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public interface IHandler<T>
    {
        T Read(Stream inputStream);
        void Write(T model, Stream outputStream);
        string FileExtension { get; }
    }
}
