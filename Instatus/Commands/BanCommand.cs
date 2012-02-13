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
using Instatus;

namespace Instatus.Commands
{
    public class BanCommand : IWebCommand
    {
        public string Name
        {
            get
            {
                return "Ban";
            }
        }
        
        public WebLink GetLink(dynamic viewModel, UrlHelper urlHelper)
        {
            User user = User.From(viewModel);
                       
            if (user.Status.Match(WebStatus.Banned))
            {
                return new WebLink()
                {
                    Title = WebPhrase.RemoveFlag("banned"),
                    Uri = urlHelper.Action("Command", new { id = viewModel.Id, commandName = Name, commandValue = WebStatus.Approved })
                };
            }
            else
            {
                return new WebLink()
                {
                    Title = WebPhrase.Flag("banned"),
                    Uri = urlHelper.Action("Command", new { id = viewModel.Id, commandName = Name, commandValue = WebStatus.Banned })
                };
            }
        }

        public bool Execute(dynamic viewModel, UrlHelper urlHelper, RouteData routeData, NameValueCollection requestParams)
        {
            var user = User.From(viewModel);
            var userId = user != null ? user.Id : routeData.Id();
            var status = requestParams.Value<WebStatus>("commandValue");
            
            using (var db = WebApp.GetService<IBaseDataContext>())
            {
                db.Users.Find(userId).Status = status.ToString();
                db.SaveChanges();
            }

            return true;
        }
    }
}
