using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Instatus.Web.Startup), "PreApplicationStart")]
[assembly: WebActivator.PostApplicationStartMethod(typeof(Instatus.Web.Startup), "PostApplicationStart")]

namespace Instatus.Web
{
    public static class Startup
    {
        public static Action<ContainerBuilder> Build { get; set; }
        
        private static void PreApplicationStart()
        {
            
        }

        private static void PostApplicationStart()
        {
            if (Build != null)
            {
                var builder = new ContainerBuilder();

                Build(builder);

                DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
            }
        }
    }
}
