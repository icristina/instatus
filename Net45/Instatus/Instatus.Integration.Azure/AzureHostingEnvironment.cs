using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Security;
using Instatus.Core;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure;

namespace Instatus.Integration.Azure
{
    public class AzureHostingEnvironment : IHostingEnvironment
    {
        public string RootPath
        {
            get
            {
                return RoleEnvironment.GetLocalResource(WellKnown.LocalResources.Output).RootPath;
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
            return CloudConfigurationManager.GetSetting(key);
        }
    }
}
