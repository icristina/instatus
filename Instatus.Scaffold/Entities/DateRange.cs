using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    [ComplexType]
    public class DateRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateRange()
        {
            Start = SqlDateTime.MinValue.Value;
            End = SqlDateTime.MaxValue.Value;
        }
    }
}