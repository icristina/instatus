using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Models
{
    public class Document : ICreated
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public DateTime Created { get; set; }
        public DateTime Published { get; set; }
        public IDictionary<string, object> Metadata { get; set; }
        public IList<Section> Sections { get; set; }
        public string[] Tags { get; set; }

        public Document()
        {
            Metadata = new Dictionary<string, object>();
            Created = DateTime.UtcNow;
            Sections = new List<Section>();
        }
    }
}
