using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;
using Instatus.Data;
using Instatus.Models;
using System.ComponentModel.Composition;

namespace Instatus.Adapters
{
    // add application notifications
    [Export(typeof(IContentAdapter))]
    [PartCreationPolicy(CreationPolicy.NonShared)]   
    public class NotificationsAdapter : IContentAdapter
    {
        private IPageContext pageContext;
        
        public void Process(IContentItem contentItem, string hint)
        {
            if (contentItem is IUserGeneratedContent) // add page notifications to extensions, to allow ExtensionsAll<Notification>
            {
                var userGeneratedContent = (IUserGeneratedContent)contentItem;

                if (userGeneratedContent.Replies != null)
                {
                    contentItem.Extensions.Notifications = userGeneratedContent.Replies.OfType<Notification>().ToList();
                }
            }
            
            var applicationNotifications = WebCache.Value(() =>
            {
                using (var context = WebApp.GetService<IApplicationContext>())
                {
                    return context.Messages.OfType<Notification>().Where(n => n.Page is Application).ToList().Randomize();
                }
            }, "ApplicationNotifications");

            contentItem.Extensions.ApplicationNotifications = applicationNotifications;
        }

        [ImportingConstructor]
        public NotificationsAdapter(IPageContext pageContext)
        {
            this.pageContext = pageContext;
        }
    }
}