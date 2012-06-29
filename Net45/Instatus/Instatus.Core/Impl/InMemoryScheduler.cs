using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core.Impl
{
    public class InMemoryScheduler
    {
        private ILogger logger;
        private int delay;
        private IList<Action> actions = new List<Action>();

        private async void ProcessActionsAsync()
        {
            while (true)
            {
                Parallel.ForEach(actions, action =>
                {
                    try
                    {
                        action.Invoke();
                    }
                    catch(Exception exception)
                    {
                        if (logger != null)
                            logger.Log(exception, null);
                    }
                });

                await TaskEx.Delay(delay);
            }
        }

        public void Subscribe(Action action)
        {
            actions.Add(action);
        }

        public void Start(int delay)
        {
            this.delay = delay;

            Task.Factory.StartNew(() => ProcessActionsAsync());
        }

        public InMemoryScheduler(ILogger logger)
        {
            this.logger = logger;
        }
    }
}
