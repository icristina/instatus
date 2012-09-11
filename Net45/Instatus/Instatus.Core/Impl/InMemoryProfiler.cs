using Instatus.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryProfiler : IProfiler
    {
        private InMemoryQueue<Entry> queue;

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
            queue = new InMemoryQueue<Entry>(limit);
        }
    }

    internal class InMemoryProfilerStep : AbstractProfilerStep
    {
        private IQueue<Entry> queue;
        
        public override void WriteStart(string message)
        {
            // only save completion profiler messages
        }

        public override void WriteEnd(string message)
        {
            queue.Enqueue(new Entry(message));
        }

        public InMemoryProfilerStep(string stepName, IQueue<Entry> queue)
            : base(stepName)
        {
            this.queue = queue;
        }
    }
}
