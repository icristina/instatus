using Instatus.Core;
using Instatus.Core.Impl;
using Instatus.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace Instatus.Integration.Server
{
    public class FileSystemContentManager : IContentManager
    {
        private ISessionData sessionData;

        private string ReadFile(string locale, string key, string formatString = "~/App_Data/{0}.{1}.html") 
        {
            var virtualPath = string.Format(formatString, key, locale);
            var absolutePath = HostingEnvironment.MapPath(virtualPath);

            return File.ReadAllText(absolutePath);
        }

        public Document Get(string key)
        {
            string content;
            
            try 
            {
                content = ReadFile(sessionData.Locale, key);
            } 
            catch 
            {
                try
                {
                    content = ReadFile(WellKnown.Locale.UnitedStates, key);
                }
                catch
                {
                    try
                    {
                        content = ReadFile(string.Empty, key, "~/App_Data/{0}.html");
                    }
                    catch
                    {
                        content = string.Empty;
                    }
                }
            }

            return new Document()
            {
                Description = content
            };
        }

        public IEnumerable<Document> Query(Filter filter)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdate(string key, Document contentItem)
        {
            throw new NotImplementedException();
        }

        public void Delete(string key)
        {
            throw new NotImplementedException();
        }

        public FileSystemContentManager(ISessionData sessionData)
        {
            this.sessionData = sessionData;
        }
    }
}
