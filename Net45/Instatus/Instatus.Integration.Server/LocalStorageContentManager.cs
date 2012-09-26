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
using Instatus.Core.Extensions;

namespace Instatus.Integration.Server
{
    public class LocalStorageContentManager : IContentManager
    {
        private IDocumentHandler documentHandler;
        private ILocalStorage localStorage;
        private ISessionData sessionData;

        public Document Get(string key)
        {
            foreach (var virtualPath in ResolveVirtualPaths(key))
            {
                try
                {
                    using (var inputStream = new MemoryStream()) 
                    {
                        localStorage.Stream(virtualPath, inputStream);
                        return documentHandler.Parse(inputStream);
                    }
                }
                catch
                {

                }
            }

            return null;
        }

        public IEnumerable<Document> Query(Filter filter)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdate(string key, Document document)
        {
            var virtualPath = ResolveVirtualPaths(key).First();

            using(var inputStream = new MemoryStream()) 
            {
                documentHandler.Write(document, inputStream);
                localStorage.Save(virtualPath, inputStream);
            }
        }

        public void Delete(string key)
        {
            foreach (var path in ResolveVirtualPaths(key))
            {
                try
                {
                    localStorage.Delete(path);
                }
                catch
                {

                }
            }
        }

        private string[] ResolveVirtualPaths(string key)
        {
            return new string[] 
            {
                string.Format("~/App_Data/{0}.{1}.{2}", key, sessionData.Locale, documentHandler.FileExtension),
                string.Format("~/App_Data/{0}.{1}.{2}", key, WellKnown.Locale.UnitedStates, documentHandler.FileExtension),
                string.Format("~/App_Data/{0}.{1}", key, documentHandler.FileExtension)
            };
        }

        public LocalStorageContentManager(IDocumentHandler documentHandler, ILocalStorage localStorage, ISessionData sessionData)
        {
            this.documentHandler = documentHandler;
            this.localStorage = localStorage;
            this.sessionData = sessionData;
        }
    }
}
