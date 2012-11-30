using Instatus.Core.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Hosting;
using Instatus.Core.Extensions;

namespace Instatus.Integration.Mvc
{
    public class EmbeddedResourceVirtualPathProvider : VirtualPathProvider
    {
        private Assembly assembly;
        private string[] resourceNames;
        private string assemblyName;

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (Previous.FileExists(virtualPath))
                return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);

            return new EmbeddedResourceCacheDependency();
        }

        public override bool FileExists(string virtualPath)
        {
            if (Previous.FileExists(virtualPath))
                return true;

            var resourceName = GetResourceName(virtualPath);

            return resourceNames.Contains(resourceName);
        }

        public override bool DirectoryExists(string virtualDir)
        {
            return Previous.DirectoryExists(virtualDir);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (Previous != null && Previous.FileExists(virtualPath))
                return Previous.GetFile(virtualPath);

            return new EmbeddedVirtualFile(assembly, GetResourceName(virtualPath), virtualPath);
        }

        private String GetResourceName(String virtualPath)
        {
            var parts = virtualPath
                .TrimStart(PathBuilder.RelativeChars)
                .Split('/')
                .ToArray();
            
            return assemblyName 
                + "." 
                + string.Join(".", parts);
        }

        public EmbeddedResourceVirtualPathProvider(Type type)
        {
            assembly = type.Assembly;
            resourceNames = assembly.GetManifestResourceNames();
            assemblyName = assembly.GetName().Name;
        }
    }

    public class EmbeddedResourceCacheDependency : CacheDependency
    {
        public new bool HasChanged
        {
            get
            {
                return false;
            }
        }
    }

    public class EmbeddedVirtualFile : VirtualFile
    {
        private Assembly assembly;
        private String name;

        public override Stream Open()
        {
            return assembly.GetManifestResourceStream(name);
        }

        public EmbeddedVirtualFile(Assembly assembly, String name, String virtualPath)
            : base(virtualPath)
        {
            this.assembly = assembly;
            this.name = name;
        }
    }
}
