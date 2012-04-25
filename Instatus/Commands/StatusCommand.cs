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
using System.Data.Entity;

namespace Instatus.Commands
{
    public abstract class StatusCommand<T> : IWebCommand where T : class, IUserGeneratedContent
    {
        private WebStatus fromStatus;
        private string fromText;
        private WebStatus toStatus;
        private string toText;
        
        public string Name
        {
            get
            {
                return GetType().Name;
            }
        }        
        
        public WebLink GetLink(dynamic viewModel, UrlHelper urlHelper)
        {
            var userGeneratedContent = (IUserGeneratedContent)viewModel;
            
            if (!userGeneratedContent.Status.Match(toStatus))
            {
                return new WebLink()
                {
                    Title = toText,
                    Uri = urlHelper.Action("Command", new { id = viewModel.Id, commandName = Name, commandValue = toStatus })
                };
            }
            else if (!fromText.IsEmpty() && userGeneratedContent.Status.Match(toStatus))
            {
                return new WebLink()
                {
                    Title = fromText,
                    Uri = urlHelper.Action("Command", new { id = viewModel.Id, commandName = Name, commandValue = fromStatus })
                };
            }
            else
            {
                return null;
            }
        }

        public bool Execute(dynamic viewModel, UrlHelper urlHelper, RouteData routeData, NameValueCollection requestParams)
        {
            var id = routeData.Id();
            var status = requestParams.Value<WebStatus>("commandValue");
            
            using (var db = WebApp.GetService<IApplicationModel>())
            {
                if (db is DbContext)
                {
                    var context = (DbContext)db;

                    var entity = context.Set<T>().Find(id);
                    var originalValue = entity.Status;

                    entity.Status = status.ToString();

                    db.LogChange(entity, "Status", originalValue, status);
                    db.SaveChanges();
                }
            }

            return true;
        }

        public StatusCommand(WebStatus toStatus, string toText, WebStatus fromStatus, string fromText)
        {
            this.fromStatus = fromStatus;
            this.fromText = fromText;
            this.toStatus = toStatus;
            this.toText = toText;
        }

        public StatusCommand(WebStatus toStatus, string toText)
        {
            this.toStatus = toStatus;
            this.toText = toText;
        }
    }
}
