using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public static IMessageQueue<TMessage> RegisterBackgroundHandler<TMessage>(this IMessageQueue<TMessage> queue, Action<TMessage> handler, int retry = 0, int delay = 10000, int parallelism = 1)
        {
            TaskExtensions.Repeat(() =>
            {
                while (true)
                {
                    var messages = new List<TMessage>();

                    if (queue.TryDequeue(messages, parallelism))
                    {
                        Parallel.ForEach(messages, message =>
                        {
                            TaskExtensions.Retry(() => handler(message), retry);
                        });
                    }
                    else
                    {
                        break;
                    }
                }
            }, delay);

            return queue;
        }

        public static bool TryDequeue<TMessage>(this IMessageQueue<TMessage> queue, IList<TMessage> list, int batchSize = 5)
        {
            int counter = batchSize;
            TMessage message;

            while (counter > 0 && queue.TryDequeue(out message))
            {
                list.Append(message);
                counter--;
            }

            return counter < batchSize;
        }
    }
}