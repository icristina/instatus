using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Models
{
    public class Entry : ICreated
    {
        public string Text { get; private set; }
        public DateTime Created { get; private set; }

        public Entry(string text)
        {
            Text = text;
            Created = DateTime.UtcNow;
        }
    }
}
