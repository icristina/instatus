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
            NotFound,
            Confirmation,
            Complete,
            Attachments,
            Map,
            Navigation,
            Related,
            Progress,
            // actions
            MoreActions,
            LoadMore,
            ReadMore,
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
            Subscribe,
            Previous,
            Next,
            Search,
            Like,
            Comment,
            Vote,
            Close,
            Cancel,
            Choose,
            // fields
            Telephone,
            EmailAddress,
            FamilyName,
            GivenName,
            Password,
            Author,
            Published,
            Id,
            Description,
            Actions,
            Filter,
            Mode,
            Tags,
            Slug,
            // phrases
            LogOnErrorDescription,
            ErrorDescription,
            NotFoundDescription,
            SearchDescription,
            MapDescription,
            NoResults,
            // labels
            Copyright
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
            RemoveFlag,
            StartDate,
            PublishedDate,
            HtmlTitle
        }

        public class ErrorMessage
        {
            public const string InvalidFriendlyIdentifier = "InvalidFriendlyIdentifier";
            public const string InvalidEmailAddress = "InvalidEmailAddress";
            public const string RequiredEmailAddress = "RequiredEmailAddress";
            public const string RequiredGivenName = "RequiredGivenName";
            public const string RequiredFamilyName = "RequiredFamilyName";
            public const string RequiredFullName = "RequiredFullName";
            public const string DuplicateUser = "DuplicateUser";
            public const string DuplicateSubscription = "DuplicateSubscription";
            public const string VerificationTokenRejected = "VerificationTokenRejected";
            public const string AcceptTerms = "AcceptTerms";
        }
        
        public static string JustNow { get { return Localize(Common.JustNow, "just now"); } }
        public static string OneMinuteAgo { get { return Localize(Common.OneMinuteAgo, "one minute ago"); } }
        public static string Yesterday { get { return Localize(Common.Yesterday, "yesterday"); } }
        public static string Yes { get { return Localize(Common.Yes, "yes"); } }
        public static string No { get { return Localize(Common.No, "no"); } }
        public static string And { get { return Localize(Common.And, "and"); } }
        public static string TermsAndConditions { get { return Localize(Common.TermsAndConditions, "Terms and Conditions"); } }
        public static string PrivacyPolicy { get { return Localize(Common.PrivacyPolicy, "Privacy Policy"); } }
        public static string LoadMore { get { return Localize(Common.LoadMore, "Load more"); } }
        public static string ReadMore { get { return Localize(Common.ReadMore, "Read more"); } }
        public static string Latest { get { return Localize(Common.Latest, "Latest"); } }
        public static string Expand { get { return Localize(Common.Expand, "Expand"); } }
        public static string Featured { get { return Localize(Common.Featured, "Featured"); } }
        public static string Telephone { get { return Localize(Common.Telephone, "Telephone"); } }
        public static string EmailAddress { get { return Localize(Common.EmailAddress, "Email address"); } }
        public static string Password { get { return Localize(Common.Password, "Password"); } }
        public static string Author { get { return Localize(Common.Author, "Author"); } }
        public static string Published { get { return Localize(Common.Published, "Published"); } }
        public static string LogOn { get { return Localize(Common.LogOn, "Log on"); } }
        public static string LogOut { get { return Localize(Common.LogOut, "Log out"); } }
        public static string Homepage { get { return Localize(Common.Homepage, "Home"); } }
        public static string Submit { get { return Localize(Common.Submit, "Submit"); } }
        public static string Create { get { return Localize(Common.Create, "Create"); } }
        public static string Details { get { return Localize(Common.Details, "Details"); } }
        public static string Edit { get { return Localize(Common.Edit, "Edit"); } }
        public static string Delete { get { return Localize(Common.Delete, "Delete"); } }
        public static string Id { get { return Localize(Common.Id, "Id"); } }
        public static string Slug { get { return Localize(Common.Slug, "Slug"); } }
        public static string Description { get { return Localize(Common.Description, "Description"); } }
        public static string Actions { get { return Localize(Common.Actions, "Actions"); } }
        public static string All { get { return Localize(Common.All, "All"); } }
        public static string Error { get { return Localize(Common.Error, "Error"); } }
        public static string ErrorDescription { get { return Localize(Common.ErrorDescription, "Sorry. An error occured on the page."); } }
        public static string NotFound { get { return Localize(Common.NotFound, "File not found"); } }
        public static string NotFoundDescription { get { return Localize(Common.NotFoundDescription, "Sorry. The file you are looking for cannot be found or has been moved."); } }
        public static string LogOnErrorDescription { get { return Localize(Common.LogOnErrorDescription, "Sorry. Your username or password were incorrect."); } }
        public static string Filter { get { return Localize(Common.Filter, "Filter"); } }
        public static string Mode { get { return Localize(Common.Mode, "Mode"); } }
        public static string View { get { return Localize(Common.View, "View"); } }
        public static string Play { get { return Localize(Common.Play, "Play"); } }
        public static string Tags { get { return Localize(Common.Tags, "Tags"); } }
        public static string Subscribe { get { return Localize(Common.Subscribe, "Subscribe"); } }
        public static string Confirmation { get { return Localize(Common.Confirmation, "Confirmation"); } }
        public static string Complete { get { return Localize(Common.Complete, "Complete"); } }
        public static string Previous { get { return Localize(Common.Previous, "Previous"); } }
        public static string Next { get { return Localize(Common.Next, "Next"); } }
        public static string Attachments { get { return Localize(Common.Attachments, "Attachments"); } }
        public static string Search { get { return Localize(Common.Search, "Search"); } }
        public static string SearchDescription { get { return Localize(Common.SearchDescription, "Enter keyword or phrase"); } }
        public static string Map { get { return Localize(Common.Map, "Map"); } }
        public static string MapDescription { get { return Localize(Common.MapDescription, "Select a location"); } }
        public static string NoResults { get { return Localize(Common.NoResults, "Sorry. No results matched your query."); } }
        public static string Navigation { get { return Localize(Common.Navigation, "Navigation"); } }
        public static string Related { get { return Localize(Common.Related, "Related"); } }
        public static string Like { get { return Localize(Common.Like, "Like"); } }
        public static string Vote { get { return Localize(Common.Vote, "Vote"); } }
        public static string Comment { get { return Localize(Common.Comment, "Comment"); } }
        public static string FamilyName { get { return Localize(Common.FamilyName, "Surname"); } }
        public static string GivenName { get { return Localize(Common.GivenName, "First name"); } }
        public static string Progress { get { return Localize(Common.Progress, "Progress"); } }
        public static string Close { get { return Localize(Common.Close, "Close"); } }
        public static string Cancel { get { return Localize(Common.Cancel, "Cancel"); } }
        public static string Choose { get { return Localize(Common.Choose, "Choose"); } }
        public static string MoreActions { get { return Localize(Common.MoreActions, "More"); } }
        public static string Copyright { get { return Localize(Common.Copyright, "&copy " + DateTime.UtcNow.Year); } }

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
        public static string StartDate(DateTime date) { return Format(FormatString.StartDate, "{0:dd/MM/yyyy}", date); }
        public static string PublishedDate(DateTime date) { return Format(FormatString.PublishedDate, "{0:dd/MM/yyyy}", date); }
        public static string HtmlTitle(string value) { return Format(FormatString.HtmlTitle, "{0}", value); }

        public static string InvalidFriendlyIdentifier { get { return Localize(ErrorMessage.InvalidFriendlyIdentifier, "Invalid friendly url, slug or identifier"); } }
        public static string InvalidEmailAddress { get { return Localize(ErrorMessage.InvalidEmailAddress, "Please enter a valid email address"); } }
        public static string RequiredEmailAddress { get { return Localize(ErrorMessage.RequiredEmailAddress, "Please enter a valid email address"); } }
        public static string RequiredGivenName { get { return Localize(ErrorMessage.RequiredGivenName, "Please enter your first name"); } }
        public static string RequiredFamilyName { get { return Localize(ErrorMessage.RequiredFamilyName, "Please enter your surname"); } }
        public static string RequiredFullName { get { return Localize(ErrorMessage.RequiredFullName, "Please enter your name"); } }
        public static string DuplicateSubscription { get { return Localize(ErrorMessage.DuplicateSubscription, "You have already subscribed"); } }
        public static string DuplicateUser { get { return Localize(ErrorMessage.DuplicateUser, "You have already registered"); } }
        public static string VerificationTokenRejected { get { return Localize(ErrorMessage.VerificationTokenRejected, "The verification token has been rejected"); } }
        public static string AcceptTerms { get { return Localize(ErrorMessage.AcceptTerms, "Please confirm you have read and accept the terms and conditions"); } }

        public static string Localize(object phraseOrKey, string defaultPhrase = null, bool keyAsDefault = true)
        {
            var resource = HttpContext.GetGlobalResourceObject(null, phraseOrKey.ToString());

            if (!keyAsDefault)
                return resource.IsEmpty() ? defaultPhrase : resource.ToString();
            
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