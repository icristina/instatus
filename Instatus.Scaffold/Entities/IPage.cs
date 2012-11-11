using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Scaffold.Entities
{
    public interface IPage : ICreated, IPayload
    {
        int Id { get; set; }
        string Name { get; set; }
        string Content { get; set; }
        string Picture { get; set; }
        string Locale { get; set; }
        string Category { get; set; }
        DateRange Publish { get; set; }
    }
}
