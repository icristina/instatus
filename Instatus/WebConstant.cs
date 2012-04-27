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
            public const string Environment = "Environment";
            public const string Simulate = "Simulate";
            public const string DatabaseInitialize = "DatabaseInitialize";
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

        public static class Slug
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
        }

        public static class RegularExpression
        {
            public const string EmailAddress = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            public const string Alias = @"^[a-z0-9-]+$";
            public const string RealName = @"^[A-Za-z-\s]+$";
        }
    }
}