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
using Instatus;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Instatus.Areas.Editor.Models;

namespace Instatus.Areas.Editor.Controllers
{
    public class NewsViewModel : BaseViewModel<News, IDataContext>
    {
        [Category("Overview")]
        [Display(Order = 1)]
        public OverviewViewModel<News> Overview { get; set; }

        [Category("Body")]
        [Display(Order = 2)]
        public DocumentViewModel<News> Document { get; set; }

        [Category("Call To Action")]
        [Display(Order = 3)]
        public CallToActionViewModel<News> CallToAction { get; set; }

        [Category("Publishing")]
        [Display(Order = 4)]
        public PublishingViewModel<News> Publishing { get; set; }

        public NewsViewModel()
        {
            Overview = new OverviewViewModel<News>();
            Document = new DocumentViewModel<News>();
            CallToAction = new CallToActionViewModel<News>();
            Publishing = new PublishingViewModel<News>();
        }
    }
    
    [Authorize(Roles = "Editor")]
    [Description("News")]
    public class NewsController : ScaffoldController<NewsViewModel, News, IDataContext, int>
    {
        public override IEnumerable<News> Query(IEnumerable<News> set, WebQuery query)
        {
            return set.ByRecency();
        }
    }
}
