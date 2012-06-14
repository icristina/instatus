using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Web;

namespace Instatus.Services
{
    public class DebugProfilingService : IProfilingService
    {
        public IDisposable Start(string taskName)
        {
            return new DebugProfilingTask(taskName);
        }

        internal class DebugProfilingTask : IDisposable
        {
            private string taskName;
            private Stopwatch stopWatch;
            
            public DebugProfilingTask(string taskName)
            {
                this.stopWatch = new Stopwatch();
                this.taskName = taskName;

                stopWatch.Start();

                Debug.WriteLine("Started '{0}'", taskName, null);
            }

            public void Dispose()
            {
                stopWatch.Stop();
                
                Debug.WriteLine("Completed '{0}' taking {1} milliseconds", taskName, stopWatch.ElapsedMilliseconds);                
            }
        }
    }
}