using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Security;
using Instatus.Core;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Instatus.Integration.Azure
{
    public class AzureHostingEnvironment : IHostingEnvironment
    {
        public string RootPath
        {
            get
            {
                return RoleEnvironment.GetLocalResource("Output").RootPath;
            }
        }

        public string BaseAddress
        {
            get
            {
                return GetAppSetting("BaseAddress");
            }
        }

        public string GetAppSetting(string key)
        {
            try
            {
                return RoleEnvironment.GetConfigurationSettingValue(key);
            }
            catch
            {
                return ConfigurationManager.AppSettings[key];
            }
        }
    }
}
