using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Instatus.Core.Extensions;

namespace Instatus.Core.Impl
{
    public class InMemoryMessageBus : IMessageBus
    {
        private IDictionary<string, object> bufferBlocks = new ConcurrentDictionary<string, object>();

        private BroadcastBlock<T> GetBroadcastBlock<T>()
        {
            return bufferBlocks.GetValue<BroadcastBlock<T>>(typeof(T).Name, () => new BroadcastBlock<T>(t => t));
        }

        public void Subscribe<T>(Action<T> action)
        {
            GetBroadcastBlock<T>().LinkTo(new ActionBlock<T>(action));
        }

        public void Publish<T>(T message)
        {
            GetBroadcastBlock<T>().Post(message);
        }
    }
}
