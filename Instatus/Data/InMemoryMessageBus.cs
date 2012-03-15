using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Data
{
    // http://brentedwards.net/2010/04/13/roll-your-own-simple-message-bus-event-aggregator/    
    public class InMemoryMessageBus : IMessageBus
    {
        private Dictionary<Type, List<Object>> subscribers = new Dictionary<Type, List<Object>>();

        public void Subscribe<TMessage>(Action<TMessage> handler)
        {
            if (subscribers.ContainsKey(typeof(TMessage)))
            {
                var handlers = subscribers[typeof(TMessage)];

                handlers.Add(handler);
            }
            else
            {
                var handlers = new List<Object>();
                
                handlers.Add(handler);
                subscribers[typeof(TMessage)] = handlers;
            }
        }

        public void Unsubscribe<TMessage>(Action<TMessage> handler)
        {
            if (subscribers.ContainsKey(typeof(TMessage)))
            {
                var handlers = subscribers[typeof(TMessage)];

                handlers.Remove(handler);

                if (handlers.Count == 0)
                {
                    subscribers.Remove(typeof(TMessage));
                }
            }
        }

        public void Publish<TMessage>(TMessage message)
        {
            if (subscribers.ContainsKey(typeof(TMessage)))
            {
                var handlers = subscribers[typeof(TMessage)];
                
                foreach (Action<TMessage> handler in handlers)
                {
                    handler.Invoke(message);
                }
            }
        }
    }
}