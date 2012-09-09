using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public interface ISession
    {
        string SiteName { get; } // hostname or name
        string Locale { get; set; }
    }
}
