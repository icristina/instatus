using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Instatus.Scaffold
{
    public static class PaginationExtensions
    {
        public static MvcHtmlString Pagination<T>(this HtmlHelper<T> htmlHelper, IPaged paged) 
        {
            return htmlHelper.Partial("_Pagination", paged);
        }

        public static int TotalPageCount(int totalItemCount, int pageSize)
        {
            return totalItemCount == 0 ? 0 : (int)Math.Ceiling((double)totalItemCount / pageSize);
        }

        public static int IncrementPager(int i, int currentPage, int totalPages, int maxPagerCount)
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