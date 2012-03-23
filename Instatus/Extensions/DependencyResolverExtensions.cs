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
            MefDependencyResolver.RegisterTypes(types);
        }

        public static void UnregisterType(this IDependencyResolver dependencyResolver, Type type)
        {
            MefDependencyResolver.UnregisterType(type);
        }
    }
}