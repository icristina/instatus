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
using Instatus.Integration.Wordpress;
using Instatus.Integration.Maxmind;
using Instatus.Integration.HtmlAgilityPack;
using Instatus.Integration.Razor;
using Instatus.Core.Models;

namespace Instatus.Sample
{
    public class AutofacConfig
    {
        public static void RegisterContainer()
        {
            var containerBuilder = new ContainerBuilder();
            var assembly = typeof(AutofacConfig).Assembly;

            containerBuilder.RegisterControllers(assembly);
            containerBuilder.RegisterSource(new ViewRegistrationSource());

            containerBuilder.RegisterType<AppSettingsStorage<Credential>>().As<IKeyValueStorage<Credential>>();
            containerBuilder.RegisterType<AspNetHosting>().As<IHosting>();
            containerBuilder.RegisterType<AspNetpreferences>().As<IPreferences>();
            containerBuilder.RegisterType<FileSystemBlobStorage>().As<IBlobStorage>();
            containerBuilder.RegisterType<WpfImaging>().As<IImaging>();
            containerBuilder.RegisterType<MockMembership>().As<IMembership>();
            containerBuilder.RegisterType<InMemoryLocalization>().As<ILocalization>();
            containerBuilder.RegisterType<AppDataStorage<Document>>().As<IKeyValueStorage<Document>>();
            containerBuilder.RegisterType<InMemoryTaxonomy>().As<ITaxonomy>();
            containerBuilder.RegisterType<DataFileGeocode>().As<IGeocode>();
            containerBuilder.RegisterType<HtmlDocumentHandler>().As<IHandler<Document>>();
            containerBuilder.RegisterType<RazorTemplating>().As<ITemplating>();
            containerBuilder.RegisterType<WordpressService>();

            var container = containerBuilder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}