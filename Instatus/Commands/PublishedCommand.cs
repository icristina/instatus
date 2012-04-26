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
using Instatus.Entities;

namespace Instatus.Commands
{
    public abstract class StatusCommand<T> : IWebCommand where T : class, IUserGeneratedContent
    {
        private Published fromStatus;
        private string fromText;
        private Published toStatus;
        private string toText;
        
        public string Name
        {
            get
            {
                return GetType().Name;
            }
        }        
        
        public Link GetLink(dynamic viewModel, UrlHelper urlHelper)
        {
            var userGeneratedContent = (IUserGeneratedContent)viewModel;
            
            if (!userGeneratedContent.Published.Match(toStatus))
            {
                return new Link()
                {
                    Title = toText,
                    Uri = urlHelper.Action("Command", new { id = viewModel.Id, commandName = Name, commandValue = toStatus })
                };
            }
            else if (!fromText.IsEmpty() && userGeneratedContent.Published.Match(toStatus))
            {
                return new Link()
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
            var status = requestParams.Value<Published>("commandValue");
            var applicationModel = DependencyResolver.Current.GetService<IApplicationModel>();

            var entity = applicationModel.Set<T>().Find(id);
            var originalValue = entity.Published;

            entity.Published = status.ToString();

            applicationModel.SaveChanges();

            return true;
        }

        public StatusCommand(Published toStatus, string toText, Published fromStatus, string fromText)
        {
            this.fromStatus = fromStatus;
            this.fromText = fromText;
            this.toStatus = toStatus;
            this.toText = toText;
        }

        public StatusCommand(Published toStatus, string toText)
        {
            this.toStatus = toStatus;
            this.toText = toText;
        }
    }
}
