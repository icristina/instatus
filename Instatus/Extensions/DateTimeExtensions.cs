using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;

namespace Instatus
{
    public static class DateTimeExtensions
    {
        // http://codeclimber.net.nz/archive/2007/07/10/convert-a-unix-timestamp-to-a-.net-datetime.aspx
        public static DateTime ToUnixTime(this double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        public static double ToUnixTimestamp(this DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public static DateTime Next(this DateTime date, WebMode mode)
        {
            switch (mode)
            {
                case WebMode.Year:
                    return date.AddYears(1);
                case WebMode.Month:
                    return date.AddMonths(1);
                case WebMode.Week:
                    return date.AddDays(7);
                default:
                    return date.AddDays(1);
            }
        }

        public static DateTime Previous(this DateTime date, WebMode mode)
        {
            switch (mode)
            {
                case WebMode.Year:
                    return date.AddYears(-1);
                case WebMode.Month:
                    return date.AddMonths(-1);
                case WebMode.Week:
                    return date.AddDays(-7);
                default:
                    return date.AddDays(-1);
            }
        }

        public static string ToString(this DateTime date, WebMode mode)
        {
            switch (mode)
            {
                case WebMode.Year:
                    return date.ToString("yyyy");
                case WebMode.Month:
                    return date.ToString("MMMM yyyy");
                case WebMode.Week:
                    return date.ToString("dddd dd MMMM yyyy");
                default:
                    return date.ToString("dddd dd MMMM yyyy");
            }
        }
    }
}