using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime GetWeekStartDate(this DateTime dateTime)
        {
            return dateTime
                .AddDays(-(dateTime.DayOfWeek - DayOfWeek.Monday))
                .Date;
        }

        public static DateTime GetWeekEndDateTime(this DateTime dateTime)
        {
            return dateTime
                .GetWeekStartDate()
                .AddDays(7)
                .AddMilliseconds(-1);
        }

        public static DateTime GetMonthStartDate(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime GetMonthEndDateTime(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month + 1, 1)
                .AddMilliseconds(-1);
        }
    }
}
