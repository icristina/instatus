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
    }
}
