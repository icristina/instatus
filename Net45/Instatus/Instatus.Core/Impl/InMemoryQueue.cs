using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Instatus.Core.Impl
{
    public class InMemoryQueue<T> : IQueue<T>
    {
        private int limit;
        private ConcurrentQueue<T> queue = new ConcurrentQueue<T>();
        private ReaderWriterLockSlim readLock = new ReaderWriterLockSlim();
        private Action<List<T>> flushAction;

        public void Trim(int size)
        {
            T removedMessage;

            while (queue.Count >= size)
            {
                queue.TryDequeue(out removedMessage);
            }
        }

        public List<T> Flush(int size)
        {
            var list = new List<T>();
            
            while (queue.Count >= size)
            {
                T removedMessage;
                
                if (queue.TryDequeue(out removedMessage))
                {
                    list.Add(removedMessage);
                }
            }

            return list;
        }

        public void Enqueue(T message)
        {
            if (limit > 0 && queue.Count >= limit && !readLock.IsWriteLockHeld)
            {
                try
                {
                    readLock.EnterWriteLock();

                    if (flushAction != null)
                    {
                        flushAction(Flush(1));
                    }
                    else
                    {
                        Trim(limit);
                    }
                }
                finally
                {
                    readLock.ExitWriteLock();
                }
            }

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

        public InMemoryQueue(int limit, Action<List<T>> flushAction = null)
        {
            this.limit = limit;
            this.flushAction = flushAction;
        }
    }
}
