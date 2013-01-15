using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface IGlobalization
    {
        DateTime Now { get; }
        CultureInfo[] Cultures { get; }
        CultureInfo DefaultCulture { get; }
    }
}
