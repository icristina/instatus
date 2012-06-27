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
}