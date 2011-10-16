using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public enum WebAction
    {
        Index,
        Details,
        Edit,
        Create,
        Delete
    }

    public static class WebActions
    {
        public static WebAction[] ForMember = new WebAction[] { WebAction.Index, WebAction.Details };
        public static WebAction[] ForAuthor = new WebAction[] { WebAction.Index, WebAction.Details, WebAction.Edit, WebAction.Create };
        public static WebAction[] ForEditor = new WebAction[] { WebAction.Index, WebAction.Details, WebAction.Edit, WebAction.Create, WebAction.Delete };
    }
}