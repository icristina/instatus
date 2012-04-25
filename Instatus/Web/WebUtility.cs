using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Models;

namespace Instatus.Web
{
    public static class WebUtility
    {
        public static SelectList CreateSelectList<TSelectItem>(IEnumerable<TSelectItem> data, object selectedValue = null, IEnumerable<string> labels = null)
        {
            if (labels == null)
                labels = data.Select(s => s.ToString()).ToList();

            return new SelectList(data.Zip(labels, (d, l) => new Parameter(d, l)), "Name", "Content", selectedValue);
        }
    }
}