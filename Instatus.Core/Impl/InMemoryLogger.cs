using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryLogger : ILogger
    {
        private InMemoryQueue<InMemoryError> queue;

        public IQueryable<ICreated> Items
        {
            get
            {
                return queue.Items.AsQueryable();
            }
        }

        public void Log(Exception exception, IDictionary<string, string> properties)
        {
            var inMemoryError = new InMemoryError()
            {
                Exception = exception.Message,
                StackTrace = exception.StackTrace,
                Properties = properties
            };

            if (exception.InnerException != null)
            {
                inMemoryError.InnerException = exception.InnerException.Message;
            }
            
            queue.Enqueue(inMemoryError);
        }

        public InMemoryLogger() : this(100)
        {

        }

        public InMemoryLogger(int limit) 
        {
            queue = new InMemoryQueue<InMemoryError>(limit);
        }

        public class InMemoryError : ICreated
        {
            public string Exception { get; set; }
            public string InnerException { get; set; }
            public string StackTrace { get; set; }
            public DateTime Created { get; private set; }
            public IDictionary<string, string> Properties { get; set; }

            public InMemoryError()
            {
                Created = DateTime.UtcNow;
            }
        }
    }
}
