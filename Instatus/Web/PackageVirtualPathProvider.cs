using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web.Caching;
using System.IO;
using System.Web.Mvc;
using Instatus.Services;

namespace Instatus.Web
{   
    public class PackageVirtualPathProvider : VirtualPathProvider
    {
        private Regex matcher = new Regex(@"^~?/Packages/([^/]+)/(.+)$", RegexOptions.IgnoreCase);

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (Previous.FileExists(virtualPath))
                return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);

            return new PackageCacheDependency();
        }

        public override bool FileExists(string virtualPath)
        {
            if (Previous.FileExists(virtualPath))
                return true;

            if (!matcher.IsMatch(virtualPath))
                return false;

            var packageName = GetPackageName(virtualPath);
            var zipPath = GetPackageUri(virtualPath);
            var filePath = GetPartUris(virtualPath);
            var zipService = DependencyResolver.Current.GetService<IZipService>();

            if (zipService == null)
                return false;

            return filePath.Any(p => zipService.FileExists(zipPath, p));
        }

        public override bool DirectoryExists(string virtualDir)
        {
            return Previous.DirectoryExists(virtualDir);
        }

        public override VirtualFile GetFile(string virtualPath)
        {           
            if (Previous.FileExists(virtualPath))
                return Previous.GetFile(virtualPath);

            var packageName = GetPackageName(virtualPath);
            var zipPath = GetPackageUri(virtualPath);
            var filePath = GetPartUris(virtualPath);

            return new PackageVirtualFile(packageName, zipPath, filePath, virtualPath);
        }

        private string GetPackageName(string virtualPath)
        {
            return matcher.Matches(virtualPath)[0].Groups[1].Value;
        }

        private string GetPackageUri(string virtualPath)
        {
            return HostingEnvironment.MapPath("~/Packages/" + GetPackageName(virtualPath)  + ".zip");
        }

        private string[] GetPartUris(string virtualPath)
        {
            var packageName = GetPackageName(virtualPath);
            var filePath = matcher.Matches(virtualPath)[0].Groups[2].Value;
            return new string[] { filePath, packageName + "/" + filePath };
        }
    }

    public class PackageCacheDependency : CacheDependency
    {
        public new bool HasChanged
        {
            get
            {
                return false;
            }
        }
    }

    public class PackageVirtualFile : VirtualFile
    {
        private string packageName;
        private string zipPath;
        private string[] filePaths;
        
        public override Stream Open()
        {
            var zipService = DependencyResolver.Current.GetService<IZipService>();
            var relativePath = filePaths.First(p => zipService.FileExists(zipPath, p));   
            
            return zipService.OpenStream(zipPath, relativePath);
        }

        public PackageVirtualFile(string packageName, string zipPath, string[] filePaths, string virtualPath)
            : base(virtualPath)
        {
            this.packageName = packageName;
            this.zipPath = zipPath;
            this.filePaths = filePaths;
        }
    }
}