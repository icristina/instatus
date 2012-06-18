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