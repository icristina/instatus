using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Optimization;

namespace Instatus.Integration.Less
{
    public class LessBundleTransform : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            response.ContentType = "text/css";
            response.Content = dotless.Core.Less.Parse(response.Content);
        }
    }
}
