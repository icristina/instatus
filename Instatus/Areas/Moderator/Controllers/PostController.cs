using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Models;
using Instatus.Data;
using Instatus.Controllers;
using Instatus.Web;
using Instatus.Services;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.ComponentModel;
using Instatus.Commands;

namespace Instatus.Areas.Moderator.Controllers
{
    [Authorize(Roles = "Moderator")]
    [Description("User Generated Content")]
    public class PostController : ScaffoldController<BaseViewModel<Post>, Post, IApplicationContext, int>
    {
        public override IEnumerable<Post> Query(IEnumerable<Post> set, WebQuery query)
        {
            return set.ByRecency();
        }

        public override ICollection<IWebCommand> GetCommands(WebQuery query)
        {
            return new List<IWebCommand>()
            {
                new SpamCommand<Post>()
            };
        }

        public override void ConfigureWebView(WebView<Post> webView)
        {
            base.ConfigureWebView(webView);

            webView.Permissions = new WebAction[] { WebAction.Details };
            
            var additionalColumns = webView.Columns.ToList();

            additionalColumns.Add("Picture");

            webView.Columns = additionalColumns.ToArray();
        }
    }
}
