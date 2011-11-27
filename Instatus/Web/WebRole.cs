using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public enum WebRole
    {
        Visitor,
        Member,
        Tester,
        Executive,
        Moderator,
        Author,
        Editor,
        Administrator,
        Developer
    }

    public static class WebRoleExtensions
    {
        public static WebAction[] ToPermissions(this WebRole role)
        {
            switch (role)
            {
                case WebRole.Member:
                    return new WebAction[] { WebAction.Index, WebAction.Details };
                case WebRole.Author:
                    return new WebAction[] { WebAction.Index, WebAction.Details, WebAction.Edit, WebAction.Create };
                case WebRole.Editor:
                case WebRole.Administrator:
                case WebRole.Developer:
                    return new WebAction[] { WebAction.Index, WebAction.Details, WebAction.Edit, WebAction.Create, WebAction.Delete };
                default:
                    return null;
            }
        }
    }
}