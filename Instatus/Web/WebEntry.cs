using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Instatus.Data;

namespace Instatus.Web
{
    public interface ISyndicatable
    {
        WebEntry ToWebEntry();
    }
    
    public class WebGeospatialEntry : WebEntry
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
    
    public class WebEntry : IResource, ITimestamp
    {
        [Key]
        public string Uri { get; set; }
        public string Kind { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Caption { get; set; }
        public string User { get; set; }
        public string Source { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Rel { get; set; }

        public override string ToString()
        {
            return Title ?? Description ?? Uri;
        }
    }

    public interface IResource
    {
        string Uri { get; }
        string Title { get; }
    }

    public class ResourceComparer<TResource> : IEqualityComparer<TResource> where TResource : IResource
    {
        public bool Equals(TResource x, TResource y)
        {
            return x.Uri.Match(y.Uri);
        }

        public int GetHashCode(TResource obj)
        {
            return obj.Uri.GetHashCode();
        }
    }
}