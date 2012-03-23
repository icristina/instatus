using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;
using System.IO;
using System.Web.Hosting;
using Instatus.Web;
using System.Web.Helpers;
using Instatus.Data;

namespace Instatus.Services
{
    [Export(typeof(IBlobService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class FileSystemBlobService : IBlobService
    {       
        public static string BasePath = "~/Media/"; 
        public static string VirtualPath = "~/Media/";

        public static List<IRule<string>> FilterRules = new List<IRule<string>>()
        {
            new RegexRule(@"-(thumb|small|medium|large)\.(jpg|png|gif)", false), // no resized images, only original
            new RegexRule(@"thumbs\.db", false) // no windows thumbnail database
        };

        public static void EnsureFolderExists(string absolutePath)
        {
            var directoryName = Path.GetDirectoryName(absolutePath);
            
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
        }

        private string GetRelativePath(string contentType, string slug)
        {
            if (Path.IsPathRooted(slug))
                return slug;

            if (slug.StartsWith(VirtualPath))
                slug = slug.SubstringAfter(VirtualPath);

            if (Path.HasExtension(slug) || contentType.IsEmpty())
                return BasePath + slug;
            
            var extension = WebMimeType.GetExtension(contentType);
            var fileName = string.Format("{0}.{1}", slug ?? Generator.TimeStamp(), extension);
            return BasePath + fileName;
        }

        public string Save(string contentType, string slug, Stream stream)
        {
            var relativePath = GetRelativePath(contentType, slug);

            Save(relativePath, stream);

            return relativePath;
        }

        public static void Save(string relativePath, Stream stream) {
            var absolutePath = WebPath.Server(relativePath);

            EnsureFolderExists(absolutePath);

            using (var fs = new FileStream(absolutePath, FileMode.Create, FileAccess.Write))
            {
                stream.Flush();
                stream.Position = 0;
                stream.CopyTo(fs);
            }
        }

        public Stream Stream(string key)
        {
            try
            {
                var relativePath = GetRelativePath(null, key);
                var absolutePath = WebPath.Server(relativePath);
                
                return new FileStream(absolutePath, FileMode.Open, FileAccess.Read);
            } 
            catch(Exception error) 
            {
                return null;
            }            
        }

        public string[] Query()
        {
            return Query(BasePath)
                .Select(f =>
                {
                    return Path.GetFileName(f);
                })
                .FilterByRules(FilterRules)
                .Select(f =>
                {
                    return VirtualPath + f;
                })
                .ToArray();
        }

        public static string[] Query(string directory) 
        {
            var path = WebPath.Server(directory);
            return Directory.GetFiles(path);
        }
    }
}