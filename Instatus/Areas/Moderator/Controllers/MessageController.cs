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

namespace Instatus.Areas.Moderator.Controllers
{
    public class MessageViewModel : BaseViewModel<Message, BaseDataContext>
    {
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [Column("PageId")]
        [Display(Name = "Application")]
        public SelectList ApplicationList { get; set; }

        [ScaffoldColumn(false)]
        public int PageId { get; set; }

        [Column("Status")]
        [Display(Name = "Status")]
        public SelectList StatusList { get; set; }

        [ScaffoldColumn(false)]
        public string Status { get; set; }

        public override void Databind()
        {
            ApplicationList = new SelectList(Context.Pages.OfType<Application>().ToList(), "Id", "Name", PageId);
            StatusList = new SelectList(new WebStatus[] { WebStatus.Draft, WebStatus.Published, WebStatus.Archived }.ToStringList(), Status);
        }
    }
    
    [Authorize(Roles = "Moderator")]
    public class MessageController : ScaffoldController<MessageViewModel, Message, BaseDataContext, int>
    {
        public override IOrderedQueryable<Message> Query(IDbSet<Message> set, WebQuery query)
        {
            return set.Where(c => c.Page is Application).OrderBy(c => c.CreatedTime);
        }
        
        public override void ConfigureWebView(WebView<Message> webView)
        {
            webView.Permissions = WebRole.Administrator.ToPermissions();
            base.ConfigureWebView(webView);
        }
    }
}
