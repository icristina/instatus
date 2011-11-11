using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Web.Hosting;
using System.IO;
using System.Web.Security;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.Common;
using System.Collections;

namespace Instatus.Data
{
    public static class Generator
    {
        private static Random random = new Random();

        public static string[] EmailProviders = new string[] { "gmail.com", "hotmail.com", "yahoo.com", "facebook.com" };

        public static string[] MaleGivenNames = new string[] { "Jack", "Oliver", "Charlie", "Harry", "Alfie", "Thomas", "Joshua", "William", "James", "Daniel" };
        public static string[] FemaleGivenNames = new string[] { "Olivia", "Ruby", "Sophie", "Chloe", "Emily", "Grace", "Jessica", "Lily", "Amelia", "Evie" };
        public static string[] GivenNames = MaleGivenNames.Concat(FemaleGivenNames).ToArray();
        public static string[] FamilyNames = new string[] { "Smith", "Jones", "Taylor", "Brown", "Williams", "Wilson", "Johnson", "Davis", "Robinson", "Wright", "Thompson", "Evans", "Walker", "White", "Roberts", "Green", "Hall", "Wood", "Jackson", "Clarke" };
        
        public static string[] Words = new string[] { "lorem", "ipsum", "dolor", "sit", "amet", "consectetur", "adipisicing", "elit", "sed", "do", "eiusmod", "tempor" };
        public static string[] Sentences = new string[] {
            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
            "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
            "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
            "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
            "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.",
            "Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt.",
            "Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem.",
            "Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur?",
            "Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?"
        };
        public static string[] UnitedKingdomCities = new string[] { "London", "Birmingham", "Glasgow", "Liverpool", "Manchester", "Leeds", "Sheffield", "Edinburgh", "Bristol", "Cardiff", "Leicester", "Coventry", "Hull", "Bradford", "Belfast", "Newcastle", "Stoke", "Wolverhampton", "Nottingham", "Plymouth" };
        
        public static List<char> LowerCaseLetters = Enumerable.Range(Convert.ToInt32('a'), Convert.ToInt32('z')).Select(c => Convert.ToChar(c)).ToList();
        public static List<char> UpperCaseLetters = Enumerable.Range(Convert.ToInt32('A'), Convert.ToInt32('Z')).Select(c => Convert.ToChar(c)).ToList();

        public static double Price()
        {
            return random.Next(99, 99999) / 100; // price between £0.99 and £999.99
        }

        public static string TimeStamp()
        {
            return DateTime.UtcNow.ToString("yyyyMMddHHmmssffffff");
        }

        public static DateTime DateOfBirth(int minAge = 16)
        {
            var startDate = DateTime.Today.AddYears(-80);
            var endDate = DateTime.Today.AddYears(-minAge);
            return Date(startDate, endDate);
        }

        public static DateTime Date(DateTime startDate, DateTime endDate)
        {
            DateTime date = startDate.AddDays(random.Next((endDate - startDate).Days));
            return new DateTime(date.Year, date.Month, date.Day, random.Next(24), random.Next(60), random.Next(60));
        }

        public static DateTime HistoricalDate(int months)
        {
            return Date(DateTime.UtcNow.AddMonths(-months), DateTime.UtcNow);
        }

        public static DateTime FutureDate(int months)
        {
            return Date(DateTime.UtcNow, DateTime.UtcNow.AddMonths(months));
        }

        public static string LoadTextFile(string virtualPath)
        {
            using(var fs = File.OpenText(HostingEnvironment.MapPath(virtualPath))) {
                return fs.ReadToEnd();
            }
        }

        public static object LoadXml(Type t, string virtualPath, IEnumerable<Type> knownTypes = null)
        {
            using (var fs = new FileStream(HostingEnvironment.MapPath(virtualPath), FileMode.Open, FileAccess.Read))
            {
                return LoadXml(t, fs, knownTypes);
            }
        }

        public static object LoadXml(Type t, Stream stream, IEnumerable<Type> knownTypes = null)
        {
            DataContractSerializer ser = new DataContractSerializer(t, knownTypes);
            return ser.ReadObject(stream);
        }

        public static T LoadXml<T>(string virtualPath, IEnumerable<Type> knownTypes = null)
        {
            return (T)LoadXml(typeof(T), virtualPath, knownTypes);
        }

        public static T LoadXml<T>(Stream stream, IEnumerable<Type> knownTypes = null)
        {
            return (T)LoadXml(typeof(T), stream, knownTypes);
        }

        public static DataTable LoadCsv(string virtualPath)
        {
            var absolutePath = HostingEnvironment.MapPath(virtualPath);
            var dataTable = new DataTable();

            using (var reader = new CsvFileReader(absolutePath))
            {
                var row = new CsvRow();

                reader.ReadRow(row);

                foreach (var value in row)
                {
                    dataTable.Columns.Add(value);
                }

                while (reader.ReadRow(row))
                {
                    dataTable.Rows.Add(row.ToArray());                 
                }
            }

            return dataTable;
        }

        public static void SaveCsv(IEnumerable records, Stream stream)
        {
            if (records == null || CollectionExtensions.Count(records) == 0)
                return;

            using (var writer = new CsvFileWriter(stream))
            {
                var properties = CollectionExtensions.First(records).GetType().GetProperties();
                var header = properties.Select(p => p.Name.ToCapitalizedDelimited()).ToList();

                writer.WriteRow(header);

                foreach (var record in records)
                {
                    var fields = properties.Select(p => p.GetValue(record, null).AsString()).ToList();
                    writer.WriteRow(fields);                
                }
            }
        }

        public static string Password(int length = 6, int specialCharacters = 1)
        {
            return Membership.GeneratePassword(length, specialCharacters);
        }

        public static string EmailAddress(string key = "user")
        {
            return string.Format("{0}@{1}", key.ToSlug(), EmailProviders.Random());
        }

        public static string FullName()
        {
            return string.Format("{0} {1}", GivenNames.Random(), FamilyNames.Random());
        }

        public static string Body(int maxParagraphs = 6, int maxSentences = 3, string[] sentences = null)
        {
            var sb = new StringBuilder();
            var total = random.Next(0, maxParagraphs);

            for (var i = 0; i <= total; i++)
            {
                sb.Append("<p>");
                sb.Append(Paragraph(maxSentences, sentences));
                sb.Append("</p>");
            }

            return sb.ToString();
        }

        public static string Paragraph(int maxSentences = 3, string[] sentences = null)
        {
            var sb = new StringBuilder();
            var total = random.Next(0, maxSentences);

            for (var i = 0; i <= total; i++)
            {
                sb.Append((sentences ?? Sentences).Random());
                sb.Append(" ");
            }

            return sb.ToString();
        }

        public static string Sentence(int maxWords = 10, string[] words = null)
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
                    sb.Append(word);
                }

                sb.Append(" ");
            }

            return sb.ToString();
        }

        // http://www.blackbeltcoder.com/Articles/files/reading-and-writing-csv-files-in-c
        internal class CsvRow : List<string>
        {
            public string LineText { get; set; }

            public CsvRow() { }
            
            public CsvRow(List<string> items)
            {
                this.AddRange(items);
            }
        }

        internal class CsvFileWriter : StreamWriter
        {
            public CsvFileWriter(Stream stream)
                : base(stream)
            {
            }

            public void WriteRow(List<string> fields)
            {
                WriteRow(new CsvRow(fields));
            }

            public void WriteRow(CsvRow row)
            {
                StringBuilder builder = new StringBuilder();
                foreach (string value in row)
                {
                    // Add separator if this isn't the first value
                    if (builder.Length > 0)
                        builder.Append(',');

                    if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
                    {
                        // Special handling for values that contain comma or quote
                        // Enclose in quotes and double up any double quotes
                        builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                    }
                    else builder.Append(value);
                }
                row.LineText = builder.ToString();
                WriteLine(row.LineText);
            }
        }

        internal class CsvFileReader : StreamReader
        {
            public CsvFileReader(Stream stream)
                : base(stream)
            {
            }

            public CsvFileReader(string filename)
                : base(filename)
            {
            }

            public bool ReadRow(CsvRow row)
            {
                row.LineText = ReadLine();
                if (String.IsNullOrEmpty(row.LineText))
                    return false;

                int pos = 0;
                int rows = 0;

                while (pos < row.LineText.Length)
                {
                    string value;

                    // Special handling for quoted field
                    if (row.LineText[pos] == '"')
                    {
                        // Skip initial quote
                        pos++;

                        // Parse quoted value
                        int start = pos;

                        while (pos < row.LineText.Length)
                        {
                            // Test for quote character
                            if (row.LineText[pos] == '"')
                            {
                                // Found one
                                pos++;

                                // If two quotes together, keep one
                                // Otherwise, indicates end of value
                                if (pos >= row.LineText.Length || row.LineText[pos] != '"')
                                {
                                    pos--;
                                    break;
                                }
                            }

                            // FIX: if quoted value is across multiple lines
                            if (pos == row.LineText.Length - 1)
                            {
                                row.LineText = row.LineText + Environment.NewLine + ReadLine();
                                pos = pos + Environment.NewLine.Length;
                            }

                            pos++;
                        }
                        value = row.LineText.Substring(start, pos - start);
                        value = value.Replace("\"\"", "\"");
                    }
                    else
                    {
                        // Parse unquoted value
                        int start = pos;
                        while (pos < row.LineText.Length && row.LineText[pos] != ',')
                            pos++;
                        value = row.LineText.Substring(start, pos - start);
                    }

                    // Add field to list
                    if (rows < row.Count)
                        row[rows] = value;
                    else
                        row.Add(value);
                    rows++;

                    // Eat up to and including next comma
                    while (pos < row.LineText.Length && row.LineText[pos] != ',')
                        pos++;
                    if (pos < row.LineText.Length)
                        pos++;
                }
                // Delete any unused items
                while (row.Count > rows)
                    row.RemoveAt(rows);

                // Return true if any columns read
                return (row.Count > 0);
            }
        }
    }
}