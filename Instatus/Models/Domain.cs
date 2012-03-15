using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Instatus.Web;
using Instatus.Data;

namespace Instatus.Models
{
    public class Domain : IEntity
    {
        public int Id { get; set; }
        public string Uri { get; set; }
        public string Environment { get; set; }
        public bool IsCanonical { get; set; }

        public virtual Application Application { get; set; }
        public virtual int ApplicationId { get; set; }

        public override string ToString()
        {
            return Uri;
        }

        public Domain() { }

        public Domain(string uri, WebEnvironment environment = WebEnvironment.Production, bool isCanonical = false)
        {
            Uri = uri;
            Environment = environment.ToString();
            IsCanonical = isCanonical;
        }
    }
}