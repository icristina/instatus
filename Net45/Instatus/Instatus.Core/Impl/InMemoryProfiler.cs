using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryProfiler : IProfiler
    {
        private InMemoryQueue<BaseEntry> queue;

        public IQueryable<ICreated> Items
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
            queue = new InMemoryQueue<BaseEntry>(limit);
        }
    }

    internal class InMemoryProfilerStep : AbstractProfilerStep
    {
        private IQueue<BaseEntry> queue;
        
        public override void WriteStart(string message)
        {
            // only save completion profiler messages
        }

        public override void WriteEnd(string message)
        {
            queue.Enqueue(new BaseEntry(message));
        }

        public InMemoryProfilerStep(string stepName, IQueue<BaseEntry> queue)
            : base(stepName)
        {
            this.queue = queue;
        }
    }
}
