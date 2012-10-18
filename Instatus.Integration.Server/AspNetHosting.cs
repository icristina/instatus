﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Web.Security;
using Instatus.Core;
using System.Globalization;
using System.Threading;
using Instatus.Core.Extensions;
using System.Web;

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
                return HttpContext.Current != null && 
                    HttpContext.Current.Request.IsSecureConnection && 
                    GetAppSetting(WellKnown.AppSetting.SecureBaseAddress) != null ? 
                    GetAppSetting(WellKnown.AppSetting.SecureBaseAddress) : GetAppSetting(WellKnown.AppSetting.BaseAddress); 
            }
        }

        public string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
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

        public string ServerName
        {
            get 
            {
                return string.Empty;
            }
        }
    }
}
