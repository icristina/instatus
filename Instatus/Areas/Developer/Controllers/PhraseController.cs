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

namespace Instatus.Areas.Developer.Controllers
{
    public class PhraseViewModel : BaseViewModel<Phrase>
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    [Authorize(Roles = "Developer")]
    [Description("Localization")]
    public class PhraseScaffoldController : ScaffoldController<PhraseViewModel, Phrase, BaseDataContext, int>
    {
        public override IOrderedQueryable<Phrase> Query(IDbSet<Phrase> set, WebQuery query)
        {
            return set.OrderBy(o => o.Name);
        }

        public override void ConfigureWebView(WebView<Phrase> webView)
        {
            webView.Permissions = WebRole.Developer.ToPermissions();
            base.ConfigureWebView(webView);
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
            PubSub.Provider.Publish<ApplicationReset>(new ApplicationReset());
        }
    }
}
