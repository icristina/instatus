using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Instatus.Core;
using Instatus.Integration.Server;
using Instatus.Core.Impl;

namespace Instatus.Sample
{
    public class AutofacConfig
    {
        public static void RegisterContainer()
        {
            var containerBuilder = new ContainerBuilder();
            var assembly = typeof(AutofacConfig).Assembly;

            containerBuilder.RegisterControllers(assembly);

            containerBuilder.RegisterType<AppSettingsCredentialStorage>().As<ICredentialStorage>();
            containerBuilder.RegisterType<AspNetHostingEnvironment>().As<IHostingEnvironment>();
            containerBuilder.RegisterType<FileSystemBlobStorage>().As<IBlobStorage>();
            containerBuilder.RegisterType<WpfImaging>().As<IImaging>();
            containerBuilder.RegisterType<MockMembershipProvider>().As<IMembershipProvider>();

            var container = containerBuilder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}