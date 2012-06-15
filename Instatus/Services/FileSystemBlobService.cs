﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;
using System.IO;
using System.Web.Hosting;
using Instatus.Web;
using System.Web.Helpers;
using Instatus.Data;
using System.Net;

namespace Instatus.Services
{
    public class FileSystemBlobService : ILocalStorageService
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

        protected virtual string GetRelativePath(string contentType, string slug)
        {
            if (Path.IsPathRooted(slug))
                return slug;

            if (slug.StartsWith(VirtualPath, StringComparison.OrdinalIgnoreCase))
                slug = slug.SubstringAfter(VirtualPath);

            if (Path.HasExtension(slug) || contentType.IsEmpty())
                return Path.Combine(BasePath, slug);
            
            var extension = WebMimeType.GetExtension(contentType);
            var fileName = string.Format("{0}.{1}", slug ?? Generator.TimeStamp(), extension);

            return Path.Combine(BasePath, fileName);
        }

        public string MapPath(string virtualPath)
        {
            return HostingEnvironment.MapPath(BasePath + virtualPath);
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
            catch
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
            EnsureFolderExists(path);
            return Directory.GetFiles(path);
        }

        public Models.Request GenerateSignedRequest(string contentType, string virtualPath)
        {
            throw new NotImplementedException();
        }

        public void Copy(string virtualPath, string uri)
        {
            using (var webClient = new WebClient())
            {
                Save(virtualPath, webClient.DownloadData(uri).ToStream());
            }  
        }

        public FileSystemBlobService() { }

        public FileSystemBlobService(string basePath)
        {
            BasePath = basePath;
        }
    }
}