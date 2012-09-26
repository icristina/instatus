using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public interface ILinguistics
    {
        string Plural(string text);
        string Singular(string text);
    }
}
