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
using Instatus.Entities;
using Instatus;

namespace Instatus.Areas.Moderator.Controllers
{
    [Authorize(Roles = WebConstant.Role.Moderator)]
    [Description("User Generated Content")]
    [AddParts(Scope = WebConstant.Scope.Admin)]
    public class PostController : ScaffoldController<BaseViewModel<Page>, Page, IApplicationModel, int>
    {
        public override IEnumerable<Page> Query(IEnumerable<Page> set, Query query)
        {
            return set.Where(p => p.Kind == "Post").ByRecency();
        }

        public override ICollection<IWebCommand> GetCommands(Query query)
        {
            return new List<IWebCommand>()
            {
                new SpamCommand<Page>()
            };
        }

        public override void ConfigureWebView(WebView<Page> webView)
        {
            base.ConfigureWebView(webView);

            webView.Permissions = new string[] { "Details" };
            
            var additionalColumns = webView.Columns.ToList();

            additionalColumns.Add("Picture");

            webView.Columns = additionalColumns.ToArray();
        }
    }
}
