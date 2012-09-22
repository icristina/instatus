using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface ILocalization
    {
        string Phrase(string locale, string key);
        string Format(string locale, string key, params object[] values);
        CultureInfo[] SupportedCultures { get; }
    }
}
