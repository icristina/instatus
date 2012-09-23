using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Instatus.Integration.Wordpress
{
    public class WordpressRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return DependencyResolver.Current.GetService<WordpressService>();
        }
    }
}
