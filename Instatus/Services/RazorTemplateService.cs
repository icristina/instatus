using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.IO;
using System.ComponentModel.Composition;
using Instatus.Web;

namespace Instatus.Services
{
    [Export(typeof(ITemplateService))]
    public class RazorTemplateService : ITemplateService
    {
        public string Process(string template, dynamic data)
        {
            var context = new ControllerContext();
            var viewData = new ViewDataDictionary(data);
            var tempData = new TempDataDictionary();
            var routeData = new RouteData();

            context.HttpContext = Mock.CreateHttpContextBase();
            context.RouteData.Values.Add("controller", "Home");

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, template);
                var viewContext = new ViewContext(context, viewResult.View, viewData, tempData, sw);

                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}