using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;
using System.ComponentModel.DataAnnotations;

namespace Instatus.Models
{
    public class Milestone : Page
    {
        public Milestone() { }
        
        public Milestone(string name, DateTime dateTime) : base(name) 
        {
            PublishedTime = dateTime;
        } 
    }
}