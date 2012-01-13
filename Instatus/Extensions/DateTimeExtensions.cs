using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;
using Instatus.Data;

namespace Instatus
{
    public static class DateTimeExtensions
    {
        // http://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-in-c
        public static int Age(this DateTime dateOfBirth)
        {
            var now = DateTime.Today;
            var age = now.Year - dateOfBirth.Year;
            
            if (dateOfBirth > now.AddYears(-age)) 
                age--;

            return age;
        }
        
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

        public static string ToTimestamp(this DateTime date)
        {
            return date.ToString("yyyyMMddHHmmssffffff");
        }

        // http://www.dotnetperls.com/pretty-date
        // http://www.blackbeltcoder.com/Articles/time/a-friendly-datetime-formatter
        public static string ToRelativeString(this DateTime date)
        {
            var now = DateTime.UtcNow;
            var timespan = now.Subtract(date);

            if (timespan.TotalDays < 1)
            {
                if (timespan.TotalSeconds < 60)
                    return WebPhrase.JustNow;

                if (timespan.TotalMinutes < 2)
                    return WebPhrase.OneMinuteAgo;

                if (timespan.TotalMinutes < 60)
                    return WebPhrase.MinutesAgo(Math.Ceiling(timespan.TotalMinutes));

                if (timespan.TotalHours < 24)
                    return WebPhrase.HoursAgo(Math.Ceiling(timespan.TotalHours));
            }
            
            if (timespan.TotalDays < 2)
                return WebPhrase.Yesterday;

            if (timespan.TotalDays < 7)
                return WebPhrase.DaysAgo(Math.Ceiling(timespan.TotalDays));

            if (timespan.TotalDays < 31)
                return WebPhrase.WeeksAgo(Math.Ceiling((double)timespan.TotalDays / 7));

            if (date.Year == now.Year)
                return date.ToString("MMM d");

            return date.ToString("MMM d, yyyy");
        }

        // http://geekswithblogs.net/mnf/archive/2008/02/21/min-and-max-methods-for-datetime.aspx
        // http://stackoverflow.com/questions/1906525/c-generic-math-functions-min-max-etc
        public static DateTime Min(this DateTime t1, DateTime t2)
        {
            return Range.Min(t1, t2);
        }

        public static DateTime Max(this DateTime t1, DateTime t2)
        {
            return Range.Max(t1, t2);
        }
    }
}