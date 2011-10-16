using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Tasks
{
    public static class TaskProvider
    {
        public static void Retry(Action action, int times)
        {
            while (times-- >= 0)
            {
                try
                {
                    action();
                    break;
                } catch {

                }
            }
        }
    }
}