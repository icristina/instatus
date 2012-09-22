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
        private ISessionData sessionData;

        private object ReadFile(string locale, string key, string formatString = "~/App_Data/{0}.{1}.html") 
        {
            var virtualPath = string.Format(formatString, key, locale);
            var absolutePath = HostingEnvironment.MapPath(virtualPath);

            return File.ReadAllText(absolutePath);
        }

        public object Get(string key)
        {
            try 
            {
                return ReadFile(sessionData.Locale, key);
            } 
            catch 
            {
                try
                {
                    return ReadFile(WellKnown.Locale.UnitedStates, key);
                }
                catch
                {
                    try
                    {
                        return ReadFile(string.Empty, key, "~/App_Data/{0}.html");
                    }
                    catch
                    {
                        return null;
                    }
                }
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

        public FileSystemContentManager(ISessionData sessionData)
        {
            this.sessionData = sessionData;
        }
    }
}
