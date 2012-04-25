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
        public static string[] ToPermissions(this WebRole role)
        {
            switch (role)
            {
                case WebRole.Member:
                    return new string[] { "Index", "Details" };
                case WebRole.Author:
                    return new string[] { "Index", "Details", "Edit", "Create" };
                case WebRole.Editor:
                case WebRole.Moderator:
                case WebRole.Administrator:
                case WebRole.Developer:
                    return new string[] { "Index", "Details", "Edit", "Create", "Delete" };
                default:
                    return null;
            }
        }
    }
}