using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Security;
using Instatus.Core;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure;
using System.Globalization;
using Instatus.Core.Extensions;
using System.Web;

namespace Instatus.Integration.Azure
{
    public class AzureHosting : IHosting
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
                return HttpContext.Current != null &&
                    HttpContext.Current.Request.IsSecureConnection &&
                    GetAppSetting(WellKnown.AppSetting.SecureBaseAddress) != null ?
                    GetAppSetting(WellKnown.AppSetting.SecureBaseAddress) : GetAppSetting(WellKnown.AppSetting.BaseAddress);
            }
        }

        public string GetAppSetting(string key)
        {
            return CloudConfigurationManager.GetSetting(key);
        }

        public string ServerName
        {
            get 
            {
                return RoleEnvironment.CurrentRoleInstance.Id;
            }
        }

        public CultureInfo DefaultCulture
        {
            get
            {
                return SupportedCultures.FirstOrDefault();
            }
        }

        private CultureInfo[] supportedCultures;

        public CultureInfo[] SupportedCultures
        {
            get
            {
                return supportedCultures ?? (supportedCultures = GetAppSetting(WellKnown.AppSetting.SupportedCultures)
                        .ThrowIfNull("SupportedCultures required in AppSettings")
                        .Split(',', ';')
                        .Select(c => CultureInfo.GetCultureInfo(c))
                        .ToArray());
            }
        }
    }
}
