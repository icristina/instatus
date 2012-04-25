using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Resources;
using System.Collections;
using Instatus.Data;
using System.Web.Compilation;
using Instatus.Services;
using Instatus.Models;
using System.Web.Mvc;

namespace Instatus.Entities
{
    // http://www.west-wind.com/presentations/wwDbResourceProvider/
    // http://msdn.microsoft.com/en-us/library/aa905797.aspx
    public class DbResourceReader : IResourceReader
    {   
        public void Close()
        {

        }

        public IDictionaryEnumerator GetEnumerator()
        {
            var entries = new Dictionary<object, object>();
            var applicationModel = DependencyResolver.Current.GetService<IApplicationModel>();

            foreach (var phrase in applicationModel.Phrases.ToList())
                entries.AddNonEmptyValue(phrase.Name, phrase.Value);
            
            return entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {

        }
    }

    public class DbResourceProvider : IResourceProvider
    {
        private static ResourceSet resourceSet;
        
        public object GetObject(string resourceKey, System.Globalization.CultureInfo culture)
        {
            if(resourceSet == null)
                resourceSet = new ResourceSet(new DbResourceReader());
            
            return resourceSet.GetObject(resourceKey);
        }

        public IResourceReader ResourceReader
        {
            get { 
                return new DbResourceReader(); 
            }
        }

        public DbResourceProvider()
        {
            // TODO: re-add subscribe to clear resourceset on updates to set
        }
    }

    public class DbResourceProviderFactory : ResourceProviderFactory
    {       
        public override IResourceProvider CreateGlobalResourceProvider(string classKey)
        {
            return new DbResourceProvider();
        }

        public override IResourceProvider CreateLocalResourceProvider(string virtualPath)
        {
            return new DbResourceProvider();
        }
    }
}