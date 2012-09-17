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
        
        public static string[] EmailProviders = new string[] { "gmail.com", "hotmail.com", "yahoo.com", "facebook.com" };
        public static string[] MaleFirstNames = new string[] { "Jack", "Oliver", "Charlie", "Harry", "Alfie", "Thomas", "Joshua", "William", "James", "Daniel" };
        public static string[] FemaleFirstNames = new string[] { "Olivia", "Ruby", "Sophie", "Chloe", "Emily", "Grace", "Jessica", "Lily", "Amelia", "Evie" };
        public static string[] FirstNames = MaleFirstNames.Concat(FemaleFirstNames).ToArray();
        public static string[] LastNames = new string[] { "Smith", "Jones", "Taylor", "Brown", "Williams", "Wilson", "Johnson", "Davis", "Robinson", "Wright", "Thompson", "Evans", "Walker", "White", "Roberts", "Green", "Hall", "Wood", "Jackson", "Clarke" };

        public static IUser CreateMockUser()
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
    }
}
