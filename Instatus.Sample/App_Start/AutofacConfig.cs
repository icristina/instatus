﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Instatus.Core;
using Instatus.Integration.Autofac;
using Instatus.Integration.Server;
using Instatus.Core.Impl;
using Instatus.Integration.Maxmind;
using Instatus.Integration.HtmlAgilityPack;
using Instatus.Integration.Razor;
using Instatus.Core.Models;
using Instatus.Scaffold.Entities;
using Instatus.Integration.EntityFramework;
using Instatus.Scaffold.Models;

namespace Instatus.Sample
{
    public class AutofacConfig
    {
        public static void RegisterContainer()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<AppSettingsStorage<Credential>>().As<ILookup<Credential>>();
            containerBuilder.RegisterType<AspNetHosting>().As<IHosting>();
            containerBuilder.RegisterType<AspNetPreferences>().As<IPreferences>();
            containerBuilder.RegisterType<FileSystemBlobStorage>().As<IBlobStorage>();
            containerBuilder.RegisterType<WpfImaging>().As<IImaging>();
            containerBuilder.RegisterType<MockMembership>().As<IMembership>();
            containerBuilder.RegisterType<InMemoryLocalization>().As<ILocalization>();
            containerBuilder.Register(c => new EfEntityStorage<InstatusSamplelDb>()).As<IEntityStorage>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<SocialDbStorage>().As<IKeyValueStorage<Document>>();
            containerBuilder.RegisterType<InMemoryTaxonomy>().As<ITaxonomy>();
            containerBuilder.RegisterType<DataFileGeocode>().As<IGeocode>();
            containerBuilder.RegisterType<HtmlDocumentHandler>().As<IHandler<Document>>();
            containerBuilder.RegisterType<RazorTemplating>().As<ITemplating>();
            containerBuilder.RegisterType<MockEncryption>().As<IEncryption>();
            containerBuilder.RegisterType<BlogPostEditor>().UsingConstructor(typeof(IEntityStorage));

            containerBuilder.BootstrapMvcResolver<RouteConfig>();
        }
    }
}