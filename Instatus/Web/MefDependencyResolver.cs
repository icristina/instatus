using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Web.Mvc;
using Instatus;

namespace Instatus.Web
{
    // https://github.com/MefContrib/MefContrib/blob/master/src/MefContrib.Web.Mvc/CompositionDependencyResolver.cs
    public class MefDependencyResolver : IDependencyResolver
    {
        public const string HttpContextKey = "MefDependencyResolver_Container";       
        private static IList<Type> registeredTypes = new List<Type>();

        public static void RegisterTypes(Type[] types) {
            registeredTypes = registeredTypes
                                .Append(types)
                                .Distinct() // duplicates cause composition container to not resolve type
                                .ToList();
        }

        public static void UnregisterType(Type type)
        {
            registeredTypes.Remove(type);
        }

        public static Type[] GetTypes<T>()
        {
            return registeredTypes.Where(t => typeof(T).IsAssignableFrom(t)).ToArray();
        }
        
        public CompositionContainer Container
        {
            get
            {
                if (!HttpContext.Current.Items.Contains(MefDependencyResolver.HttpContextKey))
                {
                    HttpContext.Current.Items.Add(HttpContextKey, new CompositionContainer(new TypeCatalog(registeredTypes), true, null));
                }

                return (CompositionContainer)HttpContext.Current.Items[HttpContextKey];
            }
        }

        public object GetService(Type serviceType)
        {
            var exports = Container.GetExports(serviceType, null, null);
            
            if (exports.Any())
            {
                return exports.Last().Value; // convention of last in first out
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

    // https://github.com/MefContrib/MefContrib/blob/master/src/MefContrib.Web.Mvc/CompositionContainerLifetimeHttpModule.cs
    public class MefDependencyResolverModule : IHttpModule
    {
        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.EndRequest += DisposeContainer;
        }

        public void DisposeContainer(object sender, EventArgs e)
        {
            HttpContext.Current.Items[MefDependencyResolver.HttpContextKey].TryDispose();
        }
    }
}