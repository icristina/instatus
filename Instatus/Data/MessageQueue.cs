using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Data
{
    public class MessageQueue<TMessage>
    {
        private ConcurrentQueue<TMessage> queue = new ConcurrentQueue<TMessage>();

        public void Enqueue(TMessage message)
        {
            queue.Enqueue(message);
        }

        public MessageQueue(Action<TMessage> action, int retry = 0, int delay = 10000)
        {
            TaskExtensions.Infinite(() =>
            {
                while (true)
                {
                    TMessage message;

                    if (queue.TryDequeue(out message))
                    {
                        TaskExtensions.Retry(() => action(message), retry);
                    }
                    else
                    {
                        break;
                    }
                }
            }, delay);
        }
    }
}