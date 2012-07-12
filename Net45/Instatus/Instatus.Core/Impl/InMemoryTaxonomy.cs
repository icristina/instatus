using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryTaxonomy : ITaxonomy
    {
        private string[] tags;
        
        public string[] GetTags()
        {
            return tags;
        }

        public InMemoryTaxonomy(string[] tags)
        {
            this.tags = tags;
        }
    }
}
