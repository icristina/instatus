using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface ILocalization
    {
        string Phrase(string key);
        string Format(string key, params object[] values);
    }
}
