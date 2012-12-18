using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebPages;

namespace Instatus.Integration.Mvc
{
    public static class DisplayModeProviderExtensions
    {
        public static string GetDisplayModeId(this DisplayModeProvider displayModeProvider, HttpContextBase httpContext)
        {
            return displayModeProvider.GetAvailableDisplayModesForContext(httpContext, null)
                .FirstOrDefault()
                .DisplayModeId;
        }

        private static string[] phoneDisplayModeIds = new string[] { "Phone", "Mobile" };

        public static bool IsPhone(this DisplayModeProvider displayModeProvider, HttpContextBase httpContext)
        {
            return phoneDisplayModeIds
                .Contains(displayModeProvider.GetDisplayModeId(httpContext), StringComparer.OrdinalIgnoreCase); 
        }
    }
}
