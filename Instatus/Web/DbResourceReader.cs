using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Resources;
using System.Collections;
using Instatus.Data;

namespace Instatus.Web
{
    public class DbResourceReader : IResourceReader
    {   
        public void Close()
        {

        }

        private static Dictionary<object, object> entries;

        public IDictionaryEnumerator GetEnumerator()
        {
            if (entries == null)
            {
                entries = new Dictionary<object, object>();
                
                using (var db = BaseDataContext.Instance())
                {
                    foreach (var phrase in db.Phrases.ToList())
                        entries.Add(phrase.Name, phrase.Value);
                }
            }
            
            return entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {

        }
    }
}