using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public static class WebConstant
    {
        public static class Scope
        {
            public const string Public = "Public";
        }

        public static class DataType
        {
            public const string File = "File";
        }

        public static class Route
        {
            public const string Home = "Instatus_Web_WebRoute_Home";
            public const string Post = "Instatus_Web_WebRoute_Post";
            public const string Page = "Instatus_Web_WebRoute_Page";
            public const string Default = "Instatus_Web_WebRoute_Default";
            public const string AccountVerification = "Instatus_Web_WebRoute_AccountVerification";
            public static string PagePrefix = "site";
        }

        public static class Slug
        {
            public const string Home = "home";
        }

        public static class ContentType
        {
            public const string Jpg = "image/jpeg";
        }

        public static class ViewName
        {
            public const string Carousel = "Carousel";
        }
    }
}