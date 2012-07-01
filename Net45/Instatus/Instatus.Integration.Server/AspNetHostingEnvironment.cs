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
    public class AspNetHostingEnvironment : IHostingEnvironment
    {
        public string OutputPath
        {
            get
            {
                return GetAppSetting("OutputPath") ?? HostingEnvironment.MapPath("~/media/");
            }
        }
        
        public string BaseUri
        {
            get 
            { 
                return GetAppSetting("BaseUri"); 
            }
        }

        public string LoginUrl
        {
            get 
            {
                return GetAppSetting("LoginUrl") ?? FormsAuthentication.LoginUrl;
            }
        }

        public string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
