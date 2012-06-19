using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using Instatus.Services;
using Instatus.Web;

namespace Instatus.Data
{
    public class ParallelMessageProcessor<TMessage> : IMessageProcessor<TMessage>
    {
        private ILifetimeScope lifetimeScope;
        private int retry;
        private int delay;
        private int parallelism;

        public ParallelMessageProcessor(int retry = 0, int delay = 10000, int parallelism = 1, ILifetimeScope lifetimeScope = null)
        {
            this.lifetimeScope = lifetimeScope;
            this.retry = retry;
            this.delay = delay;
            this.parallelism = parallelism;
        }

        public void Start()
        {
            TaskExtensions.Repeat(() =>
            {
                // http://aboutcode.net/2010/11/01/start-background-tasks-from-mvc-actions-using-autofac.html
                using (var container = lifetimeScope ?? WebApp.GetContainer())
                {
                    var messageQueue = container.Resolve<IMessageQueue<TMessage>>();
                    var loggingService = container.Resolve<ILoggingService>();

                    while (true)
                    {
                        var messages = new List<TMessage>();

                        if (messageQueue.TryDequeue(messages, parallelism))
                        {
                            Parallel.ForEach(messages, message =>
                            {
                                TaskExtensions.Retry(() =>
                                {
                                    var task = container.Resolve<ITask<TMessage>>();
                                    task.Process(message);
                                },
                                retry,
                                loggingService);
                            });
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }, delay);
        }
    }
}