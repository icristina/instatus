using dotless.Core.configuration;
using dotless.Core.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Optimization;
using Instatus.Core;

namespace Instatus.Integration.Less
{
    // http://jvance.com/blog/2012/08/31/ASPdotNETMVCBundlingOfBootstrapLESSSource.xhtml
    // https://github.com/dotless/dotless/blob/master/src/dotless.Core/configuration/DotlessConfiguration.cs
    // http://www.asp.net/mvc/tutorials/mvc-4/bundling-and-minification
    public class LessBundleTransform : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            var dotlessConfiguration = new DotlessConfiguration()
            {
                CacheEnabled = false,
                MinifyOutput = false,
                ImportAllFilesAsLess = true,
                LessSource = typeof(VirtualFileReader),
                Logger = typeof(DiagnosticsLogger)
            };
            
            response.ContentType = WellKnown.ContentType.Css;
            response.Content = dotless.Core.Less.Parse(response.Content, dotlessConfiguration);
        }
    }
}
