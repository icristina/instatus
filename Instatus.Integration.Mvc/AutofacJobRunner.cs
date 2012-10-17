using Autofac;
using Autofac.Integration.Mvc;
using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Integration.Mvc
{
    public static class AutofacJobRunner
    {
        public static void Run<T>() where T : IJob
        {
            using (ILifetimeScope container = AutofacDependencyResolver.Current.ApplicationContainer.BeginLifetimeScope())
            {
                try
                {
                    container.Resolve<T>().Execute();
                }
                catch(Exception exception)
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
