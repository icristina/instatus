using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface ITextTemplating
    {
        void Render(string viewName, object viewData, Stream outputStream);
    }
}
