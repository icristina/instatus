using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface IHostingEnvironment
    {
        string BaseUri { get; }
        string LoginUrl { get; }
        string GetAppSetting(string key);
    }
}
