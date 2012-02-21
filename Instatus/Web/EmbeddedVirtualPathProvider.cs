using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web.Caching;
using System.IO;
using System.Reflection;

namespace Instatus.Web
{
    public class EmbeddedVirtualPathProvider : VirtualPathProvider
    {
        private Assembly assembly = Assembly.GetExecutingAssembly();
        
        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if(Previous.FileExists(virtualPath))
                return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);

            return new EmbeddedCacheDependency();
        }

        public override bool FileExists(string virtualPath)
        {
            return Previous.FileExists(virtualPath) || assembly.GetManifestResourceNames().Contains(GetResourceName(virtualPath));
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
            var amendedPath = virtualPath.TrimStart('~');           
            var parts = amendedPath.Split('/')
                            .ToList();

            for (var i = 0; i < parts.Count; i++)
            {
                parts[i] = parts[i].ToPascalCase();
            }

            return assembly.ManifestModule.Name.SubstringBefore(".") + string.Join(".", parts);
        }
    }

    public class EmbeddedCacheDependency : CacheDependency
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