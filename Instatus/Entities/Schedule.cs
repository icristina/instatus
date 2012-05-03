using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Instatus.Entities
{
    [ComplexType]
    public class Schedule
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}