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
            return dateTime.AddDays(-(dateTime.DayOfWeek - DayOfWeek.Monday)).Date;
        }
    }
}
