using Autofac.Integration.Mvc;
using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Instatus.Integration.Autofac.AutofacGlobalContainer), "Start")]
namespace Instatus.Integration.Autofac
{
    public class AutofacGlobalContainer : IGlobalContainer
    {
        public IContainer CreateContainer()
        {
            return new Instatus.Integration.Autofac.AutofacContainer(AutofacDependencyResolver.Current.ApplicationContainer.BeginLifetimeScope());
        }

        private static void Start() 
        {
            AppContext.GlobalContainer = new AutofacGlobalContainer();
        }
    }
}
