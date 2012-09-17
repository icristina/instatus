using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Extensions
{
    public static class PaginationExtensions
    {
        public static int TotalPageCount(this IPaged paged)
        {
            return paged.TotalItemCount == 0 ? 0 : (int)Math.Ceiling((double)paged.TotalItemCount / paged.PageSize);
        }

        public static int IncrementPager(this int i, int currentPage, int totalPages, int maxPagerCount)
        {
            var original = i;
            int next;
            var pagerOffset = maxPagerCount / 2;

            if (i == 0 && totalPages > maxPagerCount && currentPage > pagerOffset)
            {
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

            return next == original ? next + 1 : next;
        }
    }
}
