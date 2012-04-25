using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.Mvc;
using System.IO;

namespace Instatus.Web
{
    public static class Mock
    {
        // http://blogs.teamb.com/craigstuntz/2010/09/10/38638/
        public class ViewDataContainer : IViewDataContainer
        {
            public ViewDataDictionary ViewData
            {
                get;
                set;
            }

            public ViewDataContainer(ViewDataDictionary viewDataDictionary)
            {
                ViewData = viewDataDictionary;
            }
        }
        
        public static HttpContextBase CreateHttpContextBase()
        {
            return new HttpContextWrapper(new HttpContext(
                new HttpRequest("", WebPath.BaseUri.ToString(), ""),
                new HttpResponse(new StringWriter())
            ));
        }

        public static HtmlHelper<T> CreateHtmlHelper<T>(T viewModel)
        {
            var viewDataDictionary = new ViewDataDictionary(viewModel);
            var viewDataContainer = new ViewDataContainer(viewDataDictionary);
            var viewContext = new ViewContext();

            viewContext.HttpContext = CreateHttpContextBase();

            return new HtmlHelper<T>(viewContext, viewDataContainer);
        }
    }
}