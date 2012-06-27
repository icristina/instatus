using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core.Impl
{
    public class InMemoryQueue<T> : IQueue<T>
    {
        private int limit;
        private ConcurrentQueue<T> queue = new ConcurrentQueue<T>();

        public void TrimSize(int size)
        {
            while (queue.Count >= size)
            {
                T removedMessage;
                queue.TryDequeue(out removedMessage);
            }
        }

        public void Enqueue(T message)
        {
            if (limit > 0)
                TrimSize(limit);           
            
            queue.Enqueue(message);
        }

        public bool TryDequeue(out T message)
        {
            return queue.TryDequeue(out message);
        }

        public IEnumerable<T> Items
        {
            get
            {
                return queue.AsEnumerable();
            }
        }

        public InMemoryQueue() : this(0)
        { 
        
        }

        public InMemoryQueue(int limit)
        {
            this.limit = limit;
        }
    }
}
