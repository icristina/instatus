using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Instatus
{
    public static class TaskExtensions
    {
        public static Task IgnoreExceptions(this Task task)
        {
            task.ContinueWith(c => { var ignored = c.Exception; },
                TaskContinuationOptions.OnlyOnFaulted |
                TaskContinuationOptions.ExecuteSynchronously);
            return task;
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
    }
}