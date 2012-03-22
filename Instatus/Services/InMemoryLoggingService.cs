using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;
using Instatus.Web;
using System.ComponentModel.Composition;

namespace Instatus.Services
{
    [Export(typeof(ILoggingService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]    
    public class InMemoryLoggingService : ILoggingService
    {
        private static Queue<WebEntry> queue = new LimitedQueue<WebEntry>(1000);
        
        public void Log(Exception error)
        {
            queue.Enqueue(new WebEntry() {
                CreatedTime = DateTime.UtcNow,
                Description = error.ToHtml(),
                Uri = error.GetUri()
            });
        }

        public IQueryable<ITimestamp> Query()
        {
            return queue.AsQueryable().Cast<ITimestamp>();
        }
    }
}