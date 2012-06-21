using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core.InMemory
{
    public class InMemoryQueue<T> : IQueue<T>
    {
        private ConcurrentQueue<T> queue = new ConcurrentQueue<T>();
        
        public void Enqueue(T message)
        {
            queue.Enqueue(message);
        }

        public bool TryDequeue(out T message)
        {
            return queue.TryDequeue(out message);
        }
    }
}
