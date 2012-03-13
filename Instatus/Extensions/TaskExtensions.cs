using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

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

        public static void Infinite(this Action action, int milliseconds = 1000)
        {
            var task = new Task(action, TaskCreationOptions.LongRunning)
                            .Delay(milliseconds)
                            .IgnoreExceptions()
                            .ContinueWith(t =>
                            {
                                Infinite(action, milliseconds);
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
                catch
                {

                }
            }
        }

        // Task.Delay is available in .NET 4.5
        // https://gist.github.com/1024860
        private static readonly ConcurrentDictionary<Task, Timer> timers = new ConcurrentDictionary<Task, Timer>();

        public static Task Delay(this Task task, int milliseconds)
        {
            var timer = new Timer(_ => Start(task), null, milliseconds, -1L);

            timers.AddOrUpdate(task, timer, (k, t) => t);
            
            return task;
        }

        private static void Start(Task task)
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