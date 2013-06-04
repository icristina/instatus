using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.OData
{
    public class ODataResponse<T>
    {
        [DataMember(Name = "odata.count")]
        public int? Count { get; set; }

        [DataMember(Name = "value")]
        public List<T> Value { get; set; }
    }
}
