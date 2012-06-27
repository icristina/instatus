using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class DebugProfiler : IProfiler
    {
        public IDisposable Step(string label)
        {
            return new DebugProfilerStep(label);
        }
    }

    internal class DebugProfilerStep : AbstractProfilerStep
    {
        public DebugProfilerStep(string stepName)
            : base(stepName)
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

    public abstract class AbstractProfilerStep : IDisposable
    {
        public Stopwatch Stopwatch { get; private set; }
        public string StepName { get; private set; }

        public AbstractProfilerStep(string stepName)
        {
            Stopwatch = new Stopwatch();
            StepName = stepName;

            Stopwatch.Start();
            WriteStart(string.Format("Started '{0}'", stepName, null));
        }

        public void Dispose()
        {
            Stopwatch.Stop();
            WriteEnd(string.Format("Completed '{0}' taking {1} milliseconds", StepName, Stopwatch.ElapsedMilliseconds));
        }

        public abstract void WriteStart(string message);
        public abstract void WriteEnd(string message);
    }
}