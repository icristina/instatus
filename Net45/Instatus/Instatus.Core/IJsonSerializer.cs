using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public interface IJsonSerializer
    {
        T Parse<T>(string json);
        string Stringify(object graph);
    }
}
