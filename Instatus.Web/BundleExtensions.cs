using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Optimization;

namespace Instatus.Web
{
    public static class BundleExtenions
    {
        public static BundleCollection AddTheme(this BundleCollection bundleCollection, string name, params string[] paths)
        {
            var styles = new Bundle(
                        "~/css/" + name,
                        new LessTransform()
                        .Then(new CssMinify()));

            foreach (var path in paths)
            {
                var virtualPath = VirtualPathUtility.IsAppRelative(path) ? path : "~/Content/" + path;

                styles.AddFile(virtualPath, false);
            }

            bundleCollection.Add(styles);

            return bundleCollection;
        }

        // https://gist.github.com/1998379
        // https://github.com/philipproplesch/Web.Optimization/tree/master/src/Web.Optimization/Extensions
        public static IBundleTransform Then(
            this IBundleTransform instance, IBundleTransform then)
        {
            return new ChainedBundle(instance, then);
        }
    }

    internal class ChainedBundle : IBundleTransform
    {
        private readonly IBundleTransform _instance;
        private readonly IBundleTransform _then;

        public ChainedBundle(
            IBundleTransform instance, IBundleTransform then)
        {
            _instance = instance;
            _then = then;
        }

        public void Process(BundleContext context, BundleResponse response)
        {
            _instance.Process(context, response);
            _then.Process(context, response);
        }
    }
}
