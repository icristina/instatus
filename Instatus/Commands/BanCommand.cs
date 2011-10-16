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
    public class BanCommand : IWebCommand
    {
        public WebLink GetLink(dynamic viewModel, UrlHelper urlHelper)
        {
            if ((viewModel.Status as string).Match(WebStatus.Banned))
            {
                return new WebLink()
                {
                    Title = "Unmark as banned",
                    Uri = urlHelper.Action("Command", new { id = viewModel.Id, status = WebStatus.Approved })
                };
            }
            else
            {
                return new WebLink()
                {
                    Title = "Mark as banned",
                    Uri = urlHelper.Action("Command", new { id = viewModel.Id, status = WebStatus.Banned })
                };
            }
        }

        public bool Execute(dynamic viewModel, RouteData routeData, NameValueCollection requestParams)
        {
            var id = routeData.Id();
            var status = requestParams.Value<WebStatus>("status");
            
            using (var db = BaseDataContext.Instance())
            {
                db.Users.Find(id).Status = status.ToString();
                db.SaveChanges();
            }

            return true;
        }
    }
}
