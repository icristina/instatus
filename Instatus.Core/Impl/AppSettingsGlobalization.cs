using Instatus.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core.Impl
{
    public class AppSettingsGlobalization : IGlobalization
    {
        private IHosting hosting;
        
        public DateTime Now
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        public CultureInfo DefaultCulture
        {
            get
            {
                return Cultures.FirstOrDefault();
            }
        }

        private CultureInfo[] cultures;

        public CultureInfo[] Cultures
        {
            get
            {
                return cultures ?? (cultures = hosting.GetAppSetting(WellKnown.AppSetting.SupportedCultures)
                        .ThrowIfNull("SupportedCultures required in AppSettings")
                        .AsDistinctArray()
                        .Select(c => CultureInfo.GetCultureInfo(c))
                        .ToArray());
            }
        }

        public AppSettingsGlobalization(IHosting hosting)
        {
            this.hosting = hosting;
        }
    }
}
