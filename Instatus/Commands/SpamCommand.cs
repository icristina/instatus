using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Data;
using Instatus.Web;
using Instatus.Models;
using System.Web.Routing;
using System.Collections.Specialized;

namespace Instatus.Commands
{
    public class SpamCommand<T> : IWebCommand where T : class, IUserGeneratedContent
    {
        public WebLink GetLink(dynamic viewModel, UrlHelper urlHelper)
        {
            if ((viewModel.Status as string).Match(WebStatus.Spam))
            {
                return new WebLink()
                {
                    Title = "Unmark as spam",
                    Uri = urlHelper.Action("Command", new { id = viewModel.Id, status = WebStatus.Published })
                };
            }
            else
            {
                return new WebLink()
                {
                    Title = "Mark as spam",
                    Uri = urlHelper.Action("Command", new { id = viewModel.Id, status = WebStatus.Spam })
                };
            }
        }

        public bool Execute(dynamic viewModel, RouteData routeData, NameValueCollection requestParams)
        {
            var id = routeData.Id();
            var status = requestParams.Value<WebStatus>("status");
            
            using (var db = BaseDataContext.Instance())
            {
                if (status == WebStatus.Spam)
                {
                    db.MarkAsSpam<T>(id);
                }
                else
                {
                    db.MarkAsPublished<T>(id);
                }

                db.SaveChanges();
            }
            return true;
        }
    }
}
