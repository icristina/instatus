using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Models
{
    public class TagCloud : Dictionary<string, int>
    {
        public TagCloud(IDictionary<string, int> tags)
        {
            foreach (var tag in tags)
            {
                this.Add(tag.Key, tag.Value);
            }
        }
    }
}