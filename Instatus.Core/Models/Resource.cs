using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Models
{
    public class Resource
    {
        public string Bucket { get; set; }
        public string Key { get; set; }

        public Resource(string virtualPath)
        {
            var segments = virtualPath.Split('/');
            
            Bucket = segments[1];
            Key = segments[2];
        }
    }
}
