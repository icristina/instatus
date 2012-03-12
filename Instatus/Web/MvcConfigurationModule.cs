using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Web
{
    public class MvcConfigurationModule : IHttpModule
    {
        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            var ns = context.GetType().BaseType.Namespace + ".Controllers";

            ControllerBuilder.Current.DefaultNamespaces.Add(ns);
        }
    }
}