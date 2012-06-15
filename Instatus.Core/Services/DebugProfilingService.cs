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

        internal class DebugProfilingTask : AbstractProfilingTask
        {
            public DebugProfilingTask(string taskName)
                : base(taskName)
            {

            }

            public override void WriteStart(string message)
            {
                Debug.WriteLine(message);
            }

            public override void WriteEnd(string message)
            {
                Debug.WriteLine(message);
            }
        }
    }

    public abstract class AbstractProfilingTask : IDisposable
    {
        public Stopwatch Stopwatch { get; set; }
        public string TaskName { get; set; }

        public AbstractProfilingTask(string taskName)
        {
            Stopwatch = new Stopwatch();
            TaskName = taskName;

            Stopwatch.Start();
            WriteStart(string.Format("Started '{0}'", taskName, null));
        }

        public void Dispose()
        {
            Stopwatch.Stop();
            WriteEnd(string.Format("Completed '{0}' taking {1} milliseconds", TaskName, Stopwatch.ElapsedMilliseconds));            
        }

        public abstract void WriteStart(string message);
        public abstract void WriteEnd(string message);
    }
}