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
            return path.StartsWith("~") ? path : directory + path;
        }

        private static void AddFiles(Bundle bundle, string[] paths, string defaultDirectory)
        {
            var bundleOrdering = new BundleFileSetOrdering(Guid.NewGuid().ToString());

            foreach (var path in paths)
            {
                var virtualPath = MakeRelative(path, defaultDirectory);
                
                bundle.AddFile(virtualPath, false);
                bundleOrdering.Files.Add(path);
            }

            BundleTable.Bundles.Add(bundle);
            BundleTable.Bundles.FileSetOrderList.Add(bundleOrdering);
        }

        public static BundleCollection AddScriptsBundle(this BundleCollection bundleCollection, string name, params string[] paths)
        {
            var bundle = new Bundle(
                        "~/js/" + name,
                        new JsMinify());

            AddFiles(bundle, paths, "~/Scripts/");

            return bundleCollection;
        }
        
        public static BundleCollection AddStylesBundle(this BundleCollection bundleCollection, string name, params string[] paths)
        {
            var bundle = new Bundle(
                        "~/css/" + name,
                        new LessTransform()
                        .Then(new CssMinify()));

            AddFiles(bundle, paths, "~/Content/");

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
