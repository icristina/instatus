using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Data;
using System.Collections;
using Instatus.Models;

namespace Instatus.Web
{
    public interface IWebView : IEnumerable, IViewModel, IHasPermission
    {
        int TotalItemCount { get; }
        int TotalPageCount { get; }
        Query Query { get; }
        SelectList Tags { get; }
        SelectList Filter { get; }
        SelectList Mode { get; }
        SelectList Sort { get; }
        ICollection<IWebCommand> Commands { get; }
        SiteMapNodeCollection Navigation { get; }
        Document Document { get; }
        string[] Columns { get; }
    }
}