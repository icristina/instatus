using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace Instatus.Web
{
    public class LessTransform : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            response.ContentType = "text/css";
            
            var parsedContent = dotless.Core.Less.Parse(response.Content);

            if (!parsedContent.IsEmpty())
                response.Content = parsedContent;
        }
    }
}
