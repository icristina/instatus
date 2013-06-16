using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus
{
    public enum DataType
    {
        // as System.ComponentModel.DataAnnotations.DataType        
        Date,
        DateTime,
        EmailAddress,
        MultilineText,
        PhoneNumber,
        PostalCode,
        Text,
        // custom
        Number,
        Boolean
    }
}
