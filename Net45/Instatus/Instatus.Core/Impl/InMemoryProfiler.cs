using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryProfiler : IProfiler
    {
        private InMemoryQueue<InMemoryProfilerEntry> queue;

        public IQueryable<ITimestamp> Items
        {
            get
            {
                return queue.Items.AsQueryable();
            }
        }

        public IDisposable Step(string label)
        {
            return new InMemoryProfilerStep(label, queue);
        }

        public InMemoryProfiler() : this(100)
        {

        }

        public InMemoryProfiler(int limit)
        {
            queue = new InMemoryQueue<InMemoryProfilerEntry>(limit);
        }
    }

    public class InMemoryProfilerEntry : ITimestamp
    {
        public string Message { get; private set; }
        public DateTime Timestamp { get; private set; }

        public InMemoryProfilerEntry(string message)
        {
            Message = message;
            Timestamp = DateTime.UtcNow;
        }
    }

    internal class InMemoryProfilerStep : AbstractProfilerStep
    {
        private IQueue<InMemoryProfilerEntry> queue;
        
        public override void WriteStart(string message)
        {
            // only save completion profiler messages
        }

        public override void WriteEnd(string message)
        {
            queue.Enqueue(new InMemoryProfilerEntry(message));
        }

        public InMemoryProfilerStep(string stepName, IQueue<InMemoryProfilerEntry> queue)
            : base(stepName)
        {
            this.queue = queue;
        }
    }
}
