using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using Instatus.Web;
using System.Web.Mvc;

namespace Instatus
{
    public static class GlobalFilterExtensions
    {
        public static void Remove<T>(this GlobalFilterCollection filters)
        {
            foreach (var item in filters.OfType<T>().ToList())
            {
                filters.Remove(item);
            }
        }
    }
}