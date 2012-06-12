using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus
{
    public static class WebConstant
    {
        public static class AppSetting
        {
            public const string Bootstrap = "Bootstrap";
            public const string Deployment = "Deployment";
        }
        
        public static class Scope
        {
            public const string Public = "Public";
            public const string Admin = "Admin";
        }

        public static class DataType
        {
            public const string File = "File";
        }

        public static class Route
        {
            public const string Home = "Route_Home";
            public const string Post = "Route_Post";
            public const string Page = "Route_Page";
            public const string Default = "Route_Default";
            public const string AccountVerification = "Route_AccountVerification";
        }

        public static class Alias
        {
            public const string Home = "home";
            public const string Terms = "terms";
            public const string Privacy = "privacy";
        }

        public static class ContentType
        {
            public const string Html = "text/html";
            public const string Jpg = "image/jpeg";
        }

        public static class ViewName
        {
            public const string Carousel = "Carousel";
            public const string Page = "Page";
            public const string Navigation = "Navigation";
            public const string NavBar = "NavBar";
        }

        public static class QueryParameter
        {
            public const string ReturnUrl = "returnUrl";
        }

        public static class NamespacePrefix
        {
            public const string Html = "html";
            public const string OpenGraph = "og";
        }

        public static class Rel
        {
            public const string Video = "video";
            public const string Legal = "legal";
            public const string Social = "social";
            public const string Corporate = "corporate";
            public const string Attachment = "attachment";
        }

        public static class Role
        {
            public const string Developer = "Developer";
            public const string Editor = "Editor";
            public const string Moderator = "Moderator";
        }

        public static class RegularExpression
        {
            //public const string EmailAddress = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            //http://aspnetwebstack.codeplex.com/SourceControl/changeset/view/7c3d766b5b1b#src%2fMicrosoft.Web.Mvc%2fEmailAddressAttribute.cs
            public const string EmailAddress = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            public const string Alias = @"^[a-z0-9-]+$";
            public const string RealName = @"^[A-Za-z-\s]+$";
        }

        public static class LocalResources
        {
            public const string Output = "Output";
        }
    }
}