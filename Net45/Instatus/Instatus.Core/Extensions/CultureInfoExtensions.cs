using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Instatus.Core.Extensions
{
    public static class CultureInfoExtensions
    {
        public static string TwoLetterCountryCode(this CultureInfo cultureInfo)
        {
            return cultureInfo.Name.Split('-')[1];
        }

        public static CultureInfo CreateSpecificCulture(string language, string country)
        {
            try
            {
                return CultureInfo.CreateSpecificCulture(string.Format("{0}-{1}", language.ToLower(), country.ToUpper()));
            }
            catch
            {
                return null;
            }           
        }
    }
}
