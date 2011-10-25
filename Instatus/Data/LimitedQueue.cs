using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Data
{
    // http://stackoverflow.com/questions/1292/limit-size-of-queuet-in-net
    public class LimitedQueue<T> : Queue<T>
    {
        public int Limit { get; private set; }

        public LimitedQueue(int limit)
            : base(limit)
        {
            Limit = limit;
        }

        public new void Enqueue(T item)
        {
            while(Count >= Limit)
            {
                Dequeue();
            }
            base.Enqueue(item);
        }
    }
}