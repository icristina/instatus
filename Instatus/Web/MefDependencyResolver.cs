using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Web.Mvc;

namespace Instatus.Web
{
    // https://github.com/MefContrib/MefContrib/blob/master/src/MefContrib.Web.Mvc/CompositionDependencyResolver.cs
    public class MefDependencyResolver : IDependencyResolver
    {
        private static CompositionContainer Container = new CompositionContainer();
        private static Type[] Types = new Type[] { };

        public static void RegisterTypes(Type[] types) {
            Types = types;
            Container = new CompositionContainer(new TypeCatalog(types));
        }

        public static Type[] GetTypes<T>()
        {
            return Types.Where(t => typeof(T).IsAssignableFrom(t)).ToArray();
        }

        public object GetService(Type serviceType)
        {
            var exports = Container.GetExports(serviceType, null, null);
            if (exports.Any())
            {
                return exports.First().Value;
            }
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var exports = Container.GetExports(serviceType, null, null);
            if (exports.Any())
            {
                return exports.Select(e => e.Value).AsEnumerable();
            }
            return new List<object>();
        }
    }
}