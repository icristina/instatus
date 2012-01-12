using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public class WebGeospatialEntry : WebEntry
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
    
    public class WebEntry : IResource
    {
        public string Kind { get; set; }
        public string Uri { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Caption { get; set; }
        public string User { get; set; }
        public string Source { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public interface IResource
    {
        string Uri { get; }
    }

    public class WebEntryComparer : IEqualityComparer<WebEntry>
    {
        public bool Equals(WebEntry x, WebEntry y)
        {
            return x.Uri.Match(y.Uri);
        }

        public int GetHashCode(WebEntry obj)
        {
            return obj.Uri.GetHashCode();
        }
    }
}