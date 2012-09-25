using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Core.Impl;
using Instatus.Core.Models;
using Instatus.Core.Extensions;

namespace Instatus.Core.Utils
{
    public static class DataBuilder
    {
        private static Random random = new Random();

        public static string[] EmailProviders = new string[] { "gmail.com", "hotmail.com", "yahoo.com", "facebook.com" };
        public static string[] MaleFirstNames = new string[] { "Jack", "Oliver", "Charlie", "Harry", "Alfie", "Thomas", "Joshua", "William", "James", "Daniel" };
        public static string[] FemaleFirstNames = new string[] { "Olivia", "Ruby", "Sophie", "Chloe", "Emily", "Grace", "Jessica", "Lily", "Amelia", "Evie" };
        public static string[] FirstNames = MaleFirstNames.Concat(FemaleFirstNames).ToArray();
        public static string[] LastNames = new string[] { "Smith", "Jones", "Taylor", "Brown", "Williams", "Wilson", "Johnson", "Davis", "Robinson", "Wright", "Thompson", "Evans", "Walker", "White", "Roberts", "Green", "Hall", "Wood", "Jackson", "Clarke" };
        public static string[] Words = new string[] { "lorem", "ipsum", "dolor", "sit", "amet", "consectetur", "adipisicing", "elit", "sed", "do", "eiusmod", "tempor" };

        public static User CreateMockUser()
        {
            var firstName = FirstNames.Random();
            var lastName = LastNames.Random();
            var emailProvider = EmailProviders.Random();

            return new User()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = string.Format("{0}.{1}@{2}", firstName, lastName, emailProvider).ToLower()
            };
        }

        public static string CreateMockSentence(int maxWords = 20, string[] words = null)
        {
            var sb = new StringBuilder();
            var total = random.Next(0, maxWords);

            for (var i = 0; i <= total; i++)
            {
                var word = (words ?? Words).Random();

                if (i == 0)
                {
                    sb.Append(word.ToPascalCase());
                }
                else
                {
                    sb.Append(" ");
                    sb.Append(word);
                }
            }

            return sb.ToString();
        }

        private static string ToPascalCase(this string text)
        {
            return string.IsNullOrEmpty(text) ? string.Empty : text.Substring(0, 1).ToUpper() + text.Substring(1);
        }
    }
}
