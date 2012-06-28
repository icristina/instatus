﻿using System;
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
        public string BaseUri
        {
            get
            {
                return GetAppSetting("BaseUri");
            }
        }

        public string LoginUrl
        {
            get { 
                return GetAppSetting("LoginUrl") ?? FormsAuthentication.LoginUrl; 
            }
        }

        public string GetAppSetting(string key)
        {
            return RoleEnvironment.GetConfigurationSettingValue(key) ?? ConfigurationManager.AppSettings[key];
        }
    }
}
