using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;
using Instatus.Models;
using Instatus.Web;
using System.Text;
using Instatus.Data;

namespace Instatus.Services
{
    [Export(typeof(ILoggingService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DbLoggingService : ILoggingService
    {
        private IApplicationContext applicationContext;
        
        public void Log(Exception error)
        {
            applicationContext.Logs.Add(new Log()
            {
                Verb = WebVerb.Error.ToString(),
                Uri = error.GetUri(),
                Message = error.ToHtml()
            });

            applicationContext.SaveChanges();
            applicationContext.Dispose();
        }

        public IQueryable<ITimestamp> Query()
        {
            return applicationContext.Logs.Cast<ITimestamp>();
        }

        [ImportingConstructor]
        public DbLoggingService(IApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }
    }
}