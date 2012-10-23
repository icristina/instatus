using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public class AppContext
    {
        public static IGlobalContainer GlobalContainer { get; set; }
        
        public static IContainer CreateContainer()
        {
            return GlobalContainer.CreateContainer();
        }

        public static void Run<T>() where T : IJob
        {
            RunInternal(c => c.Resolve<T>().Execute());
        }

        public static void Run<TJob, TInput>(TInput input) where TJob : IJob<TInput>
        {
            RunInternal(c => c.Resolve<TJob>().Execute(input));
        }

        private static void RunInternal(Action<IContainer> action)
        {
            using (var container = CreateContainer())
            {
                try
                {
                    action(container);
                }
                catch (Exception exception)
                {
                    var logger = container.Resolve<ILogger>();

                    if (logger != null)
                    {
                        logger.Log(exception, null);
                    }
                }
            }
        }
    }
}
