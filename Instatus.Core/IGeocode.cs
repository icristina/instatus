using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public interface IGeocode
    {
        string GetCountryCode(string ipAddress);
    }
}
