using Instatus.Core;
using Instatus.Core.Impl;
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
        private ISession session;

        private object ReadFile(string locale, string key) 
        {
            var virtualPath = string.Format("~/App_Data/{0}.{1}.html", key, locale);
            var absolutePath = HostingEnvironment.MapPath(virtualPath);

            return File.ReadAllText(absolutePath);
        }

        public object Get(string key)
        {
            try 
            {
                return ReadFile(session.Locale, key);
            } 
            catch 
            {
                return ReadFile(InMemoryLocalization.DefaultLocale, key);
            }
        }

        public IEnumerable Query(IFilter filter)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdate(string key, object contentItem)
        {
            throw new NotImplementedException();
        }

        public void Delete(string key)
        {
            throw new NotImplementedException();
        }

        public FileSystemContentManager(ISession session)
        {
            this.session = session;
        }
    }
}
