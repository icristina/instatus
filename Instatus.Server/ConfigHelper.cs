using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Server
{
    public static class ConfigHelper
    {
        public static string AppSetting([CallerMemberName] string key = null)
        {
            return ConfigurationManager.AppSettings[key];
        }        
        
        public static bool AppSettingAsBoolean([CallerMemberName] string key = null)
        {
            return bool.Parse(ConfigurationManager.AppSettings[key]);
        }

        public static int AppSettingAsInteger([CallerMemberName] string key = null)
        {
            return int.Parse(ConfigurationManager.AppSettings[key]);
        }
    }
}
