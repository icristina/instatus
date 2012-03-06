using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Models;
using Instatus.Data;
using Instatus.Controllers;
using Instatus.Web;
using System.ComponentModel.Composition;

namespace Instatus.Areas.Microsite.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]    
    public class StreamController : BaseController
    {
        private IApplicationContext applicationContext;
        private IPageContext pageContext;

        public ActionResult Index(WebQuery query, string viewName = "")
        {
            ViewData.Model = GetStream(query);
            return PartialView(viewName);
        }

        private object GetStream(WebQuery query)
        {
            switch (query.Kind)
            {
                case WebKind.Link:
                    return new WebView<Link>(applicationContext.GetLinks(query), query);
                case WebKind.User:
                    return new WebView<User>(applicationContext.GetUsers(query), query);
                default:
                    return new WebView<Page>(pageContext.GetPages(query), query);
            }
        }

        public StreamController(IApplicationContext applicationContext, IPageContext pageContext)
        {
            this.applicationContext = applicationContext;
            this.pageContext = pageContext;
        }
    }
}
