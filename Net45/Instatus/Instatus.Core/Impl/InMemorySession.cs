using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemorySession : ISessionData
    {
        public string Locale { get; set; }
        public IHosting Hosting
        {
            get { throw new NotImplementedException(); }
        }
    }
}
