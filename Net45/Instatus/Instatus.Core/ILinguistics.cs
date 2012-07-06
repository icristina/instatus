using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public interface ILinguistics
    {
        string Pluralize(string text);
        string Singularize(string text);
        string Suggestions(string text);
    }
}
