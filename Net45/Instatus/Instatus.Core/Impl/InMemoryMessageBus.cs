using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Instatus.Core.Impl
{
    public class InMemoryMessageBus : IMessageBus
    {
        private ILogger logger;
        private IQueue<object> queue;
        private IList<object> actions = new List<object>();

        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;

        public int Delay { get; set; }

        private int retry = 0;

        public int Retry {
            get
            {
                return retry;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Retry must be more than 0");

                retry = value;
            }
        }

        private int concurrency = 1;

        public int Concurrency {
            get
            {
                return concurrency;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentException("Concurrency must be 1 or more");

                concurrency = value;
            }
        }

        public void Subscribe<T>(Action<T> action)
        {
            actions.Add(action);
        }

        public void Publish<T>(T message)
        {
            queue.Enqueue(message);
        }

        public IEnumerable<object> MatchActions(object message)
        {
            return actions.Where(a => a.GetType().GetGenericArguments()[0].IsAssignableFrom(message.GetType()));
        }

        public void InvokeActions(object message)
        {
            foreach (var action in MatchActions(message))
            {
                var retries = 0;

                while (retries <= Retry)
                {
                    retries++;

                    try
                    {
                        (action as dynamic).Invoke(message as dynamic);
                        break;
                    }
                    catch (Exception exception)
                    {
                        if (logger != null)
                            logger.Log(exception, null);
                    }
                }
            }
        }

        private async void ProcessPendingMessagesAsync()
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if(Delay > 0)
                    await TaskEx.Delay(Delay, cancellationToken);
                
                try
                {
                    object message;

                    while (!cancellationToken.IsCancellationRequested && queue.TryDequeue(out message))
                        InvokeActions(message);
                }
                catch (Exception exception)
                {
                    if (logger != null)
                        logger.Log(exception, null);
                }
            }
        }

        public void Start()
        {
            Stop();

            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            
            for (var i = 0; i < Concurrency; i++)
            {
                Task.Factory.StartNew(() => ProcessPendingMessagesAsync(), cancellationToken);
            }
        }

        public void Stop()
        {
            if (cancellationTokenSource != null)
                cancellationTokenSource.Cancel();
        }

        public InMemoryMessageBus(IQueue<object> queue, ILogger logger)
        {
            this.queue = queue ?? new InMemoryQueue<object>();
            this.logger = logger;
        }
    }
}
