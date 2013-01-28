using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Log : ICreated, IPayload
    {
        public int Id { get; set; }
        public int StatusCode { get; set; }

        // ICreated
        public DateTime Created { get; set; }

        // IPayload
        public string Data { get; set; }

        public Log()
        {
            Created = DateTime.UtcNow;
        }
    }
}