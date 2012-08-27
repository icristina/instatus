using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class BaseEntry : ICreated
    {
        public string Text { get; private set; }
        public DateTime Created { get; private set; }

        public BaseEntry(string text)
        {
            Text = text;
            Created = DateTime.UtcNow;
        }
    }
}
