﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public class WellKnown
    {
        public class Locale
        {
            public const string UnitedStates = "en-US";
            public const string GreatBritain = "en-GB";
            public const string Canada = "en-CA";
            public const string Australia = "en-AU";
        }

        public class RegularExpression
        {
            //http://aspnetwebstack.codeplex.com/SourceControl/changeset/view/7c3d766b5b1b#src%2fMicrosoft.Web.Mvc%2fEmailAddressAttribute.cs
            public const string EmailAddress = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            public const string Alias = @"^[a-z0-9-]+$";
            public const string RealName = @"^[A-Za-z-\s]+$";
        }

        public class LocalResources
        {
            public const string Output = "Output";
        }

        public class Alias
        {
            public const string Home = "home";
            public const string About = "about";
            public const string Terms = "terms";
            public const string Privacy = "privacy";
        }

        public class ContentType
        {
            public const string Html = "text/html";
            public const string Jpg = "image/jpeg";
        }

        public class AppSetting
        {
            public const string BaseAddress = "BaseAddress";
            public const string RootPath = "RootPath";
        }

        public class VirtualPath
        {
            public const string AppRoot = "~/";
            public const string AppData = "~/App_Data/";
            public const string Media = "~/Media/";
        }

        public class Phrase
        {
            public const string AppName = "AppName";
        }

        public class RouteName
        {
            public const string Default = "Default";
            public const string ContentPage = "ContentPage";
        }

        public class FormatString
        {
            // http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx
            public const string Date = "{0:yyyy-MM-dd}";
            public const string Timestamp = "{0:yyyy-MM-dd-HH-mm-ss-F}";
            public const string TimestampAndGuid = "{0:yyyy-MM-dd-HH-mm-ss-F}-{1}";
        }
    }
}