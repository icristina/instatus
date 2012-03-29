using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Web.Routing;
using System.Web.Mvc;
using Instatus.Models;

namespace Instatus.Web
{
    public class WebPartial : WebPart
    {
        public string ActionName { get; set; }

        public virtual object GetModel(RequestContext requestContext)
        {
            return null;
        }
    }
}