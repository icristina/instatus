using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryPreferences : IPreferences
    {
        public string Locale { get; set; }
    }
}
