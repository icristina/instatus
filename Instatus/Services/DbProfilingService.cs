using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Instatus.Entities;

namespace Instatus.Services
{
    public class DbProfilingService : IProfilingService
    {
        private IApplicationModel applicationModel;
        
        public IDisposable Start(string taskName)
        {
            return new DbProfilingTask(applicationModel, taskName);
        }

        internal class DbProfilingTask : AbstractProfilingTask
        {
            private IApplicationModel applicationModel;

            public DbProfilingTask(IApplicationModel applicationModel, string taskName) : base(taskName)
            {
                this.applicationModel = applicationModel;
            }

            public override void WriteEnd(string message)
            {
                applicationModel.Logs.Add(new Log()
                {
                    Message = message
                });
                applicationModel.SaveChanges();
            }

            public override void WriteStart(string message)
            {
                // do nothing
            }
        }

        public DbProfilingService(IApplicationModel applicationModel)
        {
            this.applicationModel = applicationModel;
        }
    }
}