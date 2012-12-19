using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Instatus.Integration.Mvc
{
    public class ClientRedirectResult : ActionResult
    {
        private string url;
        
        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.RequestContext.HttpContext.Response;

            response.ContentType = WellKnown.ContentType.Html;
            response.Write(string.Format("<script>window.top.location = '{0}';</script>", url));
        }

        public ClientRedirectResult(string url)
        {
            this.url = url;
        }
    }
}
