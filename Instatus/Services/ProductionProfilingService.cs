using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Web;

namespace Instatus.Services
{
    public class ProductionProfilingService : IProfilingService
    {
        public IDisposable Start(string taskName)
        {
            return new ProductionProfilingTask();
        }

        internal class ProductionProfilingTask : IDisposable
        {
            public ProductionProfilingTask()
            {
                // do nothing
            }

            public void Dispose()
            {
                // do nothing
            }
        }
    }
}