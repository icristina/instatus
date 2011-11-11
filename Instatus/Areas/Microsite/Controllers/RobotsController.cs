using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Controllers;
using Instatus.Data;
using System.Text;
using Instatus.Web;

namespace Instatus.Areas.Microsite.Controllers
{
    public class RobotsController : BaseController<BaseDataContext>
    {
        public ActionResult Index()
        {
            var published = Context.Applications.First().PublishedTime;
            var sb = new StringBuilder();

            if (published.HasValue && published.Value > DateTime.UtcNow)
            {
                sb.AppendLine("User-agent: *");
                sb.AppendLine("Disallow: /");
            }
            
            return Content(sb.ToString(), WebContentType.Txt);
        }
    }
}
