using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Instatus.Services
{
    public class DebugLoggingService : ILoggingService
    {
        public void Log(Exception error)
        {
            Debug.WriteLine(error.Message);
        }

        public IQueryable<Models.ITimestamp> Query()
        {
            throw new NotImplementedException();
        }
    }
}