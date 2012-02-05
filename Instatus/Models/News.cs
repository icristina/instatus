﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public class News : Page
    {
        public News() { }

        public News(string slug, string name, string body = null)
            : base(name)
        {
            Slug = slug;
            Document.Body = body;
        }
    }
}