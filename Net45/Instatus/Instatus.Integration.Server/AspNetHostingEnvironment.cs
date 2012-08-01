﻿using System;
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
        public string RootPath
        {
            get
            {
                return GetAppSetting("RootPath") ?? HostingEnvironment.MapPath("~/");
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
            return ConfigurationManager.AppSettings[key];
        }
    }
}
