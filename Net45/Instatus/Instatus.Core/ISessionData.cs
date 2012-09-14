using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public interface ISessionData
    {
        string SiteName { get; } // hostname or name
        string Locale { get; set; }
    }
}
