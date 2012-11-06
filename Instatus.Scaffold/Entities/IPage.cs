using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Scaffold.Entities
{
    public interface IPage : ICreated
    {
        int Id { get; set; }
        string Name { get; set; }
        string Content { get; set; }
        string Picture { get; set; }
        DateTime Active { get; set; }
        DateTime Start { get; set; }
        DateTime End { get; set; }
        string Locale { get; set; }
        string Category { get; set; }
    }
}
