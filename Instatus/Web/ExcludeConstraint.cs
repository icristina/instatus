using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Instatus.Web
{
    // http://codebetter.com/howarddierking/2011/05/09/using-serviceroute-with-existing-mvc-routes/    
    public class ExcludeConstraint : IRouteConstraint
    {
        public ExcludeConstraint(params string[] exclusions)
        {
            this.exclusions = exclusions;
        }

        private string[] exclusions;

        public bool Match(HttpContextBase httpContext,
          Route route,
          string parameterName,
          RouteValueDictionary values,
          RouteDirection routeDirection)
        {
            string value = values[parameterName].ToString();
            return !exclusions.Contains(value, StringComparer.CurrentCultureIgnoreCase);
        }
    }
}