using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Web;
using System.ComponentModel.Composition.Hosting;

namespace Instatus
{
    public static class DependencyResolverExtensions
    {
        public static void RegisterTypes(this IDependencyResolver dependencyResolver, params Type[] types)
        {
            if (dependencyResolver is MefDependencyResolver)
            {
                MefDependencyResolver.RegisterTypes(types);
            }
            else
            {
                throw new Exception("Only available with MefDependencyResolver");
            }
        }
    }
}