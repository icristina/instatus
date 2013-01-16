using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public static class WellKnown
    {
        public static class Locale
        {
            public const string UnitedStates = "en-US";
            public const string GreatBritain = "en-GB";
            public const string Canada = "en-CA";
            public const string Australia = "en-AU";
            public const string Germany = "de-DE";
        }

        public static class RegularExpression
        {
            //http://aspnetwebstack.codeplex.com/SourceControl/changeset/view/7c3d766b5b1b#src%2fMicrosoft.Web.Mvc%2fEmailAddressAttribute.cs
            public const string EmailAddress = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            public const string Slug = @"^[a-z0-9-]+$";
            public const string RealName = @"^[A-Za-z-\s]+$";
            public const string Locale = @"^[a-z]{2}(-[A-Z]{2})?$";
            public const string Number = @"^[0-9]+$";
        }

        public static class LocalResources
        {
            public const string Output = "Output";
        }

        public static class Alias
        {
            public const string Home = "home";
            public const string About = "about";
            public const string Terms = "terms";
            public const string Privacy = "privacy";
        }

        public static class ContentType
        {
            public const string Html = "text/html";
            public const string Jpg = "image/jpeg";
            public const string Js = "text/javascript";
            public const string Css = "text/css";
            public const string Rss = "application/rss+xml";
            public const string Csv = "text/csv";
            public const string Json = "application/json";
        }

        public static class Whitelist
        {
            public static readonly string[] JpgContentType = new string[] { WellKnown.ContentType.Jpg, "image/pjpeg" };
            public static readonly string[] JpgExtension = new string[] { ".jpeg", ".jpg" };
        }

        public static class AppSetting
        {
            public const string BaseAddress = "BaseAddress";
            public const string SecureBaseAddress = "SecureBaseAddress";
            public const string CdnBaseAddress = "CdnBaseAddress";
            public const string RootPath = "RootPath";
            public const string SupportedCultures = "SupportedCultures";
            public const string EnableTheme = "EnableTheme";
            public const string TabUrl = "TabUrl";
            public const string CanvasUrl = "CanvasUrl";
        }

        public static class VirtualPath
        {
            public const string AppRoot = "~/";
            public const string AppData = "~/App_Data/";
            public const string Media = "~/Media/";
            public const string UserGenerated = "~/UserGenerated/";
        }

        public static class Phrase
        {
            public const string AppName = "AppName";
            public const string CopyrightNotice = "CopyrightNotice";
            public const string Disclaimer = "Disclaimer";
        }

        public static class RouteName
        {
            public const string Default = "Default";
            public const string Page = "Page";
            public const string Blog = "Blog";
            public const string BlogPost = "BlogPost";
        }

        public static class RouteValue
        {
            public const string Controller = "controller";
            public const string Action = "action";
            public const string Id = "id";
            public const string Locale = "locale";
            public const string ReturnUrl = "returnUrl";
        }

        public static class FormatString
        {
            // http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx
            public const string Year = "{0:yyyy}";
            public const string Month = "{0:yyyy-MM}";
            public const string Date = "{0:yyyy-MM-dd}";
            public const string Timestamp = "{0:yyyy-MM-dd-HH-mm-ss-F}";
            public const string TimestampAndGuid = "{0:yyyy-MM-dd-HH-mm-ss-F}-{1}";
        }

        public static class Provider
        {
            public const string Facebook = "Facebook";
            public const string GoogleAnalytics = "GoogleAnalytics";
            public const string GoogleMaps = "GoogleMaps";
            public const string WindowsAzure = "WindowsAzure"; // table, queue, blob storage
            public const string Microsoft = "Microsoft"; // microsoft account
            public const string Twitter = "Twitter";
            public const string Maxmind = "Maxmind";
            public const string Amazon = "Amazon";
        }

        public static class Cookie
        {
            public const string Preferences = "Preferences";
            public const string Preview = "Preview";
        }

        public static class Preference
        {
            public const string Locale = "Locale";
            public const string Like = "Like";
        }

        public static class Kind
        {
            public const string BlogPost = "BlogPost";
            public const string WebPage = "WebPage";
            public const string Product = "Product";
            public const string Catalog = "Catalog";
            public const string CaseStudy = "CaseStudy";
            public const string Photo = "Photo";
            public const string Answer = "Answer";
        }

        public static class Recurrence
        {
            public const string Daily = "Daily";
            public const string Weekly = "Weekly";
            public const string Monthly = "Monthly";
            public const string Quarterly = "Quarterly";
            public const string Annual = "Annual";
        }

        public static class Role
        {
            public const string Member = "Member";
            public const string Editor = "Editor";
            public const string Moderator = "Moderator";
            public const string Marketer = "Marketer";
            public const string Administrator = "Administrator";

            public static string[] All = new string[] 
            {
                Member,
                Editor,
                Moderator,
                Marketer,
                Administrator
            };
        }

        public static class Conversion
        {
            public static int BytesPerMegabyte = 1048576;
        }
    }
}
