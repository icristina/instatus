using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface IHostingEnvironment
    {
        string RootPath { get; }
        string BaseAddress { get; }
        string GetAppSetting(string key);
        string ServerName { get; }
    }
}
