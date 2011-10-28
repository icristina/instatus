using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using Instatus.Web;

namespace Instatus
{
    public static class EnumExtensions
    {
        public static string ToLocalizedString(this Enum value)
        {
            return WebLocalization.Phrase(value, value.ToDescriptiveString());
        }
        
        public static string ToDescriptiveString(this Enum value)
        {
            var description = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);

            if (description.IsEmpty())
                return value.ToString();

            return ((DescriptionAttribute)description.First()).Description;
        }
    }
}