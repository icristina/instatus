using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Optimization;
using Instatus;

namespace Instatus.Web
{
    public static class BundleExtenions
    {
        private static string MakeRelative(string path, string directory)
        {
            return path.StartsWith("~") ? path : "~/" + directory + "/" + path;
        }
        
        public static BundleCollection AddScripts(this BundleCollection bundleCollection, string name, params string[] paths)
        {
            var scripts = new Bundle(
                        "~/js/" + name,
                        new JsMinify());

            foreach (var path in paths)
            {
                scripts.AddFile(MakeRelative(path, "Scripts"), false);
            }

            bundleCollection.Add(scripts);

            return bundleCollection;
        }
        
        public static BundleCollection AddTheme(this BundleCollection bundleCollection, string name, params string[] paths)
        {
            var styles = new Bundle(
                        "~/css/" + name,
                        new LessTransform()
                        .Then(new CssMinify()));

            foreach (var path in paths)
            {
                styles.AddFile(MakeRelative(path, "Content"), false);
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
