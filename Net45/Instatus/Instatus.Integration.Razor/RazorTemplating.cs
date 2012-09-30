using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Instatus.Core;

namespace Instatus.Integration.Razor
{
    public class RazorTemplating : ITemplating
    {
        public void Render(string viewName, object viewData, Stream outputStream)
        {
            var context = new ControllerContext();
            var viewDataDictionary = new ViewDataDictionary(viewData);
            var tempData = new TempDataDictionary();
            var routeData = new RouteData();

            context.HttpContext = CreateHttpContextBase();
            context.RouteData.Values.Add("controller", "Home");

            using (var streamWriter = new StreamWriter(outputStream))
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, viewDataDictionary, tempData, streamWriter);

                viewResult.View.Render(viewContext, streamWriter);
            }
        }

        private HttpContextBase CreateHttpContextBase()
        {
            return new HttpContextWrapper(new HttpContext(
                new HttpRequest("", "http://localhost", ""),
                new HttpResponse(new StringWriter())
            ));
        }
    }
}
