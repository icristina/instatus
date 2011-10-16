using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public class WebMetrics
    {
        public WebStatistic Likes { get; set; }
        public WebStatistic Checkins { get; set; }
        public WebStatistic Reports { get; set; }
        public WebStatistic Comments { get; set; }
        public WebStatistic Achievements { get; set; }
        public WebStatistic Changes { get; set; }
    }
}