using Instatus.Integration.Less;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Instatus.Sample
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/content/app.css")
                .Include("~/Content/mixins.less")
                .Include("~/Content/reset.less")
                .Include("~/Content/app.less"));

            BundleTable.EnableOptimizations = true;
        }
    }
}