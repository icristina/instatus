using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instatus.Data;
using Instatus.Services;

namespace Instatus.Entities
{
    public class DbLoggingService : ILoggingService
    {
        private IApplicationModel applicationModel;

        public void Log(Exception error)
        {
            applicationModel.Logs.Add(new Log()
            {
                Uri = error.GetUri(),
                Message = error.ToHtml()
            });

            applicationModel.SaveChanges();
        }

        public IQueryable<ITimestamp> Query()
        {
            return applicationModel.Logs.Cast<ITimestamp>();
        }

        public DbLoggingService(IApplicationModel applicationModel)
        {
            this.applicationModel = applicationModel;
        }
    }
}
