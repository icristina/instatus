using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Instatus.Integration.Mvc
{
    public class RefreshOpenerResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.RequestContext.HttpContext.Response;

            response.ContentType = WellKnown.ContentType.Html;
            response.Write("<script>window.opener.location.reload(); window.close();</script>");
        }
    }
}
