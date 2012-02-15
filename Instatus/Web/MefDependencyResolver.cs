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
        public const string HttpContextKey = "MefDependencyResolver_Container";       
        private static Type[] Types = new Type[] { };

        public static void RegisterTypes(Type[] types) {
            Types = types;
        }

        public CompositionContainer Container
        {
            get
            {
                if (!HttpContext.Current.Items.Contains(MefDependencyResolver.HttpContextKey))
                {
                    HttpContext.Current.Items.Add(HttpContextKey, new CompositionContainer(new TypeCatalog(Types)));
                }

                return (CompositionContainer)HttpContext.Current.Items[HttpContextKey];
            }
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
            if (HttpContext.Current.Items.Contains(MefDependencyResolver.HttpContextKey))
            {
                var disposable = HttpContext.Current.Items[MefDependencyResolver.HttpContextKey] as IDisposable;
                
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}