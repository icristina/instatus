using Instatus.Core.Models;
using Instatus.Core.Extensions;
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

        public static IDictionary<Type, Func<IDictionary<string, object>, object>> Binders = new Dictionary<Type, Func<IDictionary<string, object>, object>>()
        {
            { typeof(Credential), values => {
                return new Credential()
                {
                    AccountName = values.GetValue<string>("AccountName"),
                    PrivateKey = values.GetValue<string>("PrivateKey"),
                    PublicKey = values.GetValue<string>("PublicKey"),
                    Claims = (values.GetValue<string>("Claims") ?? "").Split(',')
                };
            }}
        };
    }
}
