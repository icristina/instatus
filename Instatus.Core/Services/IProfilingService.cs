using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Services;

namespace Instatus.Services
{
    public interface IProfilingService
    {
        IDisposable Start(string taskName);
    }
}

namespace Instatus
{
    public static class ProfilingServiceExtensions
    {
        public static IDisposable Start(this IProfilingService profilingService, string taskName, params string[] labels)
        {
            return profilingService.Start(taskName + " " + string.Join(" ", labels));
        }
    }
}