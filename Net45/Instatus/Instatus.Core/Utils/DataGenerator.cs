using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Utils
{
    public static class DataGenerator
    {
        private static Random random = new Random();
        public static string[] EmailProviders = new string[] { "gmail.com", "hotmail.com", "yahoo.com", "facebook.com" };
        public static string[] MaleFirstNames = new string[] { "Jack", "Oliver", "Charlie", "Harry", "Alfie", "Thomas", "Joshua", "William", "James", "Daniel" };
        public static string[] FemaleFirstNames = new string[] { "Olivia", "Ruby", "Sophie", "Chloe", "Emily", "Grace", "Jessica", "Lily", "Amelia", "Evie" };
        public static string[] FirstNames = MaleFirstNames.Concat(FemaleFirstNames).ToArray();
        public static string[] LastNames = new string[] { "Smith", "Jones", "Taylor", "Brown", "Williams", "Wilson", "Johnson", "Davis", "Robinson", "Wright", "Thompson", "Evans", "Walker", "White", "Roberts", "Green", "Hall", "Wood", "Jackson", "Clarke" };

        public static T Random<T>(this IList<T> list)
        {
            return list.ElementAt(random.Next(0, list.Count()));
        }

        public IUser User
        {
            get
            {
                var firstName = Random(FirstNames);
                var lastName = Random(LastNames);
                
                return new DataGeneratorUser()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    EmailAddress = string.Format("{0}.{1}@{2}", firstName, lastName, Random(EmailProviders)).ToLower()
                };
            }
        }
    }

    internal class DataGeneratorUser : IUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Locale { get; set; }
    }
}
