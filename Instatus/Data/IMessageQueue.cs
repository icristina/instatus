                                                                                                                                                                                                                                                  using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus;
using Instatus.Data;

namespace Instatus.Data
{
    public interface IMessageQueue<TMessage>
    {
        void Enqueue(TMessage message);
        bool TryDequeue(out TMessage message);
    }
}

namespace Instatus
{
    public static class MessageQueueExtensions
    {
        public static IMessageQueue<TMessage> RegisterBackgroundHandler<TMessage>(this IMessageQueue<TMessage> queue, Action<TMessage> handler, int retry = 0, int delay = 10000)
        {
            TaskExtensions.Repeat(() =>
            {
                while (true)
                {
                    TMessage message;

                    if (queue.TryDequeue(out message))
                    {
                        TaskExtensions.Retry(() => handler(message), retry);
                    }
                    else
                    {
                        break;
                    }
                }
            }, delay);

            return queue;
        }
    }
}