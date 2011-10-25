using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus
{
    public static class NumericExtensions
    {
        public static int IncrementPager(this int i, int currentPage, int totalPages, int maxPagerCount)
        {
            var original = i;
            int next;
            var pagerOffset = maxPagerCount / 2;       
            
            if(i == 0 && totalPages > maxPagerCount && currentPage > pagerOffset) {
                if (currentPage + pagerOffset >= totalPages)
                {
                    next = totalPages - maxPagerCount;
                }
                else
                {
                    next = currentPage - pagerOffset;            
                }
            }
            else if (i == Math.Max(currentPage + pagerOffset - 1, maxPagerCount - 1) && currentPage + pagerOffset + 1 < totalPages)
            {
                next = totalPages - 1;
            }
            else
            {
                next = i + 1;
            }

            return next == original ? next + 1 : next; // TODO: bug may return same value
        }

        public static string ToPaddedString(this int number, int length)
        {
            return string.Format("{0:D" + length + "}", number);
        }

        public static string ToOrdinalString(this int number)
        {
            switch (number % 100)
            {
                case 11:
                case 12:
                case 13:
                    return number.ToString() + "th";
            }

            switch (number % 10)
            {
                case 1:
                    return number.ToString() + "st";
                case 2:
                    return number.ToString() + "nd";
                case 3:
                    return number.ToString() + "rd";
                default:
                    return number.ToString() + "th";
            }
        }
    }
}