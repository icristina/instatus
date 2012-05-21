using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Services
{
    public interface IProfilingService
    {
        IDisposable Start(string taskName);
    }
}