using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Instatus.Integration.Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder BootstrapMvcResolver<T>(this ContainerBuilder containerBuilder)
        {
            var assembly = typeof(T).Assembly;

            containerBuilder.RegisterControllers(assembly);
            containerBuilder.RegisterFilterProvider();
            containerBuilder.RegisterSource(new ViewRegistrationSource());

            var container = containerBuilder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            return containerBuilder;
        }
    }
}
