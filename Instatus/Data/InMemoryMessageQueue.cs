using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Data
{
    public class InMemoryMessageQueue<TMessage> : IMessageQueue<TMessage>
    {
        private ConcurrentQueue<TMessage> queue = new ConcurrentQueue<TMessage>();

        public void Enqueue(TMessage message)
        {
            queue.Enqueue(message);
        }

        public bool TryDequeue(out TMessage message)
        {
            return queue.TryDequeue(out message);
        }
    }
}