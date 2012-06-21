using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core.InMemory
{
    public class InMemoryMessageBus : IMessageBus
    {
        private IQueue<object> queue = new InMemoryQueue<object>();
        private IList<object> actions = new List<object>();

        public void Subscribe<T>(Action<T> action)
        {
            actions.Add(action);
        }

        public void Publish<T>(T message)
        {
            RunActionsSynchronously(message);
        }

        private void RunActionsSynchronously<T>(T message)
        {
            foreach (var action in actions.OfType<Action<T>>())
            {
                action.Invoke(message);
            }
        }
    }
}
