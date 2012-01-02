using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public class Job : Page
    {
        public Job()
        {

        }

        public Job(string slug, string name, string body = null)
            : base(name)
        {
            Slug = slug;
            Document.Body = body;
        }
    }
}