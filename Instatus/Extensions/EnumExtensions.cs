using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Instatus
{
    public static class EnumExtensions
    {
        public static string ToDescriptiveString(this Enum value)
        {
            var description = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);

            if (description.IsEmpty())
                return value.ToString();

            return ((DescriptionAttribute)description.First()).Description;
        }
    }
}