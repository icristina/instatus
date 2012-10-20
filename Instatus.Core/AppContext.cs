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
            using (var container = CreateContainer())
            {
                try
                {
                    container.Resolve<T>().Execute();
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
