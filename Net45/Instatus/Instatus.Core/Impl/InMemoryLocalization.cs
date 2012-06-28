using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryLocalization : ILocalization
    {
        private IDictionary<string, string> phrases;
        
        public string Phrase(string key)
        {
            return phrases[key];
        }

        public string Format(string key, params object[] values)
        {
            throw new NotImplementedException();
        }

        public InMemoryLocalization(IDictionary<string, string> phrases)
        {
            this.phrases = phrases;
        }
    }
}
