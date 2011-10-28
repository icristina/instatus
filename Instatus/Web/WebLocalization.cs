using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Instatus.Web
{
    public enum WebPhrase
    {
        JustNow,
        OneMinuteAgo,
        Yesterday,
        Yes,
        No,
        And,
        TermsAndConditions,
        PrivacyPolicy,
        More,
        Latest,
        Submit,
        Expand,
        Featured
    }

    public enum WebFormatString
    {
        MinutesAgo,
        HoursAgo,
        DaysAgo,
        WeeksAgo,
        OrdinalFirst,
        OrdinalSecond,
        OrdinalThird,
        OrdinalFourth,
        Visit
    }

    public class WebLocalization
    {
        public static string JustNow { get { return Phrase(WebPhrase.JustNow, "just now"); } }
        public static string OneMinuteAgo { get { return Phrase(WebPhrase.OneMinuteAgo, "one minute ago"); } }
        public static string Yesterday { get { return Phrase(WebPhrase.Yesterday, "yesterday"); } }
        public static string Yes { get { return Phrase(WebPhrase.Yes, "yes"); } }
        public static string No { get { return Phrase(WebPhrase.No, "no"); } }
        public static string And { get { return Phrase(WebPhrase.And, "and"); } }
        public static string TermsAndConditions { get { return Phrase(WebPhrase.TermsAndConditions, "Terms and Conditions"); } }
        public static string PrivacyPolicy { get { return Phrase(WebPhrase.PrivacyPolicy, "Privacy Policy"); } }
        public static string More { get { return Phrase(WebPhrase.More, "More"); } }
        public static string Latest { get { return Phrase(WebPhrase.Latest, "Latest"); } }
        public static string Expand { get { return Phrase(WebPhrase.Expand, "Expand"); } }
        public static string Featured { get { return Phrase(WebPhrase.Featured, "Featured"); } }

        public static string MinutesAgo(double minutes) { return Format(WebFormatString.MinutesAgo, "{0} minutes ago", minutes); }
        public static string HoursAgo(double hours) { return Format(WebFormatString.HoursAgo, "{0} hours ago", hours); }
        public static string DaysAgo(double days) { return Format(WebFormatString.DaysAgo, "{0} days ago", days); }
        public static string WeeksAgo(double weeks) { return Format(WebFormatString.WeeksAgo, "{0} weeks ago", weeks); }
        public static string OrdinalFirst(int value) { return Format(WebFormatString.OrdinalFirst, "{0}st", value); }
        public static string OrdinalSecond(int value) { return Format(WebFormatString.OrdinalSecond, "{0}nd", value); }
        public static string OrdinalThird(int value) { return Format(WebFormatString.OrdinalThird, "{0}rd", value); }
        public static string OrdinalFourth(int value) { return Format(WebFormatString.OrdinalFourth, "{0}th", value); }
        public static string Visit(string name) { return Format(WebFormatString.Visit, "Visit {0}", name); }

        public static string Phrase(object phraseOrKey, string defaultPhrase = null)
        {
            var resource = HttpContext.GetGlobalResourceObject(null, phraseOrKey.ToString());
            return resource.IsEmpty() ? defaultPhrase ?? phraseOrKey.ToString() : resource.ToString();
        }

        public static string Format(object phraseOrKey, string defaultPhrase, params object[] args)
        {
            return string.Format(Phrase(phraseOrKey, defaultPhrase), args);
        }
    }
}