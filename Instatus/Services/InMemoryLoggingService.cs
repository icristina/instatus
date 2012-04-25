using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;
using Instatus.Web;
using System.ComponentModel.Composition;
using Instatus.Models;

namespace Instatus.Services
{
    [Export(typeof(ILoggingService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]    
    public class InMemoryLoggingService : ILoggingService
    {
        private static Queue<Entry> queue = new LimitedQueue<Entry>(1000);
        
        public void Log(Exception error)
        {
            queue.Enqueue(new Entry() {
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