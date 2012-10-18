using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Instatus.Core;
using Instatus.Core.Impl;

namespace Instatus.Integration.Server
{
    public class TraceProfiler : IProfiler
    {
        public IDisposable Step(string label)
        {
            return new TraceProfilerStep(label);
        }
    }

    internal class TraceProfilerStep : AbstractProfilerStep
    {
        public override void WriteStart(string message)
        {
            // only trace end messages
        }

        public override void WriteEnd(string message)
        {
            Trace.TraceInformation(message);
        }

        public TraceProfilerStep(string label)
            : base(label)
        {

        }
    }
}
