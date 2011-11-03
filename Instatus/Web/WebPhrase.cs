using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Dynamic;

namespace Instatus.Web
{
    public class WebPhrase
    {
        public enum Common
        {
            // relative time
            JustNow,
            OneMinuteAgo,
            Yesterday,
            // common
            Yes,
            No,
            And,
            All,
            // titles
            TermsAndConditions,
            PrivacyPolicy,
            Featured,
            Latest,
            Homepage,
            Error,
            // actions
            More,
            Submit,
            Expand,
            LogOn,
            LogOut,
            Create,
            Details,
            Edit,
            Delete,
            View,
            Play,
            // fields
            Telephone,
            EmailAddress,
            Password,
            Author,
            Published,
            Id,
            Description,
            Actions,
            Filter,
            Mode,
            // phrases
            ErrorMessage
        }        
        
        public enum FormatString
        {
            MinutesAgo,
            HoursAgo,
            DaysAgo,
            WeeksAgo,
            OrdinalFirst,
            OrdinalSecond,
            OrdinalThird,
            OrdinalFourth,
            Visit,
            FromAToB,
            Flag,
            RemoveFlag
        }        
        
        public static string JustNow { get { return Localize(Common.JustNow, "just now"); } }
        public static string OneMinuteAgo { get { return Localize(Common.OneMinuteAgo, "one minute ago"); } }
        public static string Yesterday { get { return Localize(Common.Yesterday, "yesterday"); } }
        public static string Yes { get { return Localize(Common.Yes, "yes"); } }
        public static string No { get { return Localize(Common.No, "no"); } }
        public static string And { get { return Localize(Common.And, "and"); } }
        public static string TermsAndConditions { get { return Localize(Common.TermsAndConditions, "Terms and Conditions"); } }
        public static string PrivacyPolicy { get { return Localize(Common.PrivacyPolicy, "Privacy Policy"); } }
        public static string More { get { return Localize(Common.More, "More"); } }
        public static string Latest { get { return Localize(Common.Latest, "Latest"); } }
        public static string Expand { get { return Localize(Common.Expand, "Expand"); } }
        public static string Featured { get { return Localize(Common.Featured, "Featured"); } }
        public static string Telephone { get { return Localize(Common.Telephone, "Telephone"); } }
        public static string EmailAddress { get { return Localize(Common.EmailAddress, "Email Address"); } }
        public static string Password { get { return Localize(Common.Password, "Password"); } }
        public static string Author { get { return Localize(Common.Author, "Author"); } }
        public static string Published { get { return Localize(Common.Published, "Published"); } }
        public static string LogOn { get { return Localize(Common.LogOn, "Log On"); } }
        public static string LogOut { get { return Localize(Common.LogOut, "Log Out"); } }
        public static string Homepage { get { return Localize(Common.Homepage, "Home"); } }
        public static string Submit { get { return Localize(Common.Submit, "Submit"); } }
        public static string Create { get { return Localize(Common.Create, "Create"); } }
        public static string Details { get { return Localize(Common.Details, "Details"); } }
        public static string Edit { get { return Localize(Common.Edit, "Edit"); } }
        public static string Delete { get { return Localize(Common.Delete, "Delete"); } }
        public static string Id { get { return Localize(Common.Id, "Id"); } }
        public static string Description { get { return Localize(Common.Description, "Description"); } }
        public static string Actions { get { return Localize(Common.Actions, "Actions"); } }
        public static string All { get { return Localize(Common.All, "All"); } }
        public static string Error { get { return Localize(Common.Error, "Error"); } }
        public static string ErrorMessage { get { return Localize(Common.ErrorMessage, "Sorry. An error occured on the page."); } }
        public static string Filter { get { return Localize(Common.Filter, "Filter"); } }
        public static string Mode { get { return Localize(Common.Mode, "Mode"); } }
        public static string View { get { return Localize(Common.View, "View"); } }
        public static string Play { get { return Localize(Common.Play, "Play"); } }

        public static string MinutesAgo(double minutes) { return Format(FormatString.MinutesAgo, "{0} minutes ago", minutes); }
        public static string HoursAgo(double hours) { return Format(FormatString.HoursAgo, "{0} hours ago", hours); }
        public static string DaysAgo(double days) { return Format(FormatString.DaysAgo, "{0} days ago", days); }
        public static string WeeksAgo(double weeks) { return Format(FormatString.WeeksAgo, "{0} weeks ago", weeks); }
        public static string OrdinalFirst(int value) { return Format(FormatString.OrdinalFirst, "{0}st", value); }
        public static string OrdinalSecond(int value) { return Format(FormatString.OrdinalSecond, "{0}nd", value); }
        public static string OrdinalThird(int value) { return Format(FormatString.OrdinalThird, "{0}rd", value); }
        public static string OrdinalFourth(int value) { return Format(FormatString.OrdinalFourth, "{0}th", value); }
        public static string Visit(string name) { return Format(FormatString.Visit, "Visit {0}", name); }
        public static string FromAToB(string placeA, string placeB) { return Format(FormatString.FromAToB, "from {0} to {1}", placeA, placeB); }
        public static string Flag(string name) { return Format(FormatString.Flag, "Mark as {0}", name); }
        public static string RemoveFlag(string name) { return Format(FormatString.RemoveFlag, "Unmark as {0}", name); }

        public static string Localize(object phraseOrKey, string defaultPhrase = null)
        {
            var resource = HttpContext.GetGlobalResourceObject(null, phraseOrKey.ToString());
            return resource.IsEmpty() ? defaultPhrase ?? phraseOrKey.ToString() : resource.ToString();
        }

        public static string Format(object phraseOrKey, string defaultPhrase, params object[] args)
        {
            return string.Format(Localize(phraseOrKey, defaultPhrase), args);
        }

        private static CustomPhrases customPhrases = new CustomPhrases();

        public static dynamic Custom
        {
            get
            {
                return customPhrases;
            }
        }

        private class CustomPhrases : DynamicObject
        {
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                result = WebPhrase.Localize(binder.Name);
                return true;
            }
        }
    }
}