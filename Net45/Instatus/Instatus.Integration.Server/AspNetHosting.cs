using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Web.Security;
using Instatus.Core;

namespace Instatus.Integration.Server
{
    public class AspNetHosting : IHosting
    {
        public string RootPath
        {
            get
            {
                return GetAppSetting(WellKnown.AppSetting.RootPath) 
                    ?? HostingEnvironment.MapPath(WellKnown.VirtualPath.AppRoot);
            }
        }
        
        public string BaseAddress
        {
            get 
            { 
                return GetAppSetting(WellKnown.AppSetting.BaseAddress); 
            }
        }

        public string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public string ServerName
        {
            get 
            {
                return string.Empty;
            }
        }
    }
}
