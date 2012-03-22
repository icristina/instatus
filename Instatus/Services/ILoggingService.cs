using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;

namespace Instatus.Services
{
    public interface ILoggingService
    {
        void Log(Exception error);
        IQueryable<ITimestamp> Query();
    }
}