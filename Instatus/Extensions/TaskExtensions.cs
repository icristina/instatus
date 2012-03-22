using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using Instatus.Web;

namespace Instatus
{
    public static class TaskExtensions
    {
        public static Task IgnoreExceptions(this Task task)
        {
            task.ContinueWith(t => { },
                TaskContinuationOptions.OnlyOnFaulted |
                TaskContinuationOptions.ExecuteSynchronously);

            return task;
        }

        // Alternative is EventLoopScheduler in Reactive Extensions
        // http://msdn.microsoft.com/en-us/library/hh229870(v=vs.103).aspx
        // http://programmers.stackexchange.com/questions/13711/servicing-background-tasks-on-a-large-site
        public static void Repeat(this Action action, int delay = 1000)
        {
            var task = new Task(action, TaskCreationOptions.LongRunning)
                            .Delay(delay)
                            .IgnoreExceptions()
                            .ContinueWith(t =>
                            {
                                Repeat(action, delay);
                            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public static void Retry(this Action action, int times)
        {
            while (times-- >= 0)
            {
                try
                {
                    action();
                    break;
                }
                catch(Exception error)
                {
                    if (times == 0)
                    {
                        WebApp.Log(error);
                    }
                }
            }
        }

        // Task.Delay is available in .NET 4.5
        // https://gist.github.com/1024860
        private static readonly ConcurrentDictionary<Task, Timer> timers = new ConcurrentDictionary<Task, Timer>();

        public static Task Delay(this Task task, int milliseconds)
        {
            var timer = new Timer(_ => StartDelayedTask(task), null, milliseconds, -1L);

            timers.AddOrUpdate(task, timer, (k, t) => t);
            
            return task;
        }

        private static void StartDelayedTask(Task task)
        {
            task.Start();

            Timer timer;
            
            if (!timers.TryRemove(task, out timer)) return;
            
            using (timer)
            {
                // Just disposing
            }
        }
    }
}