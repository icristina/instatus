using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class BaseEntry : ITimestamp
    {
        public string Text { get; private set; }
        public DateTime Timestamp { get; private set; }

        public BaseEntry(string text)
        {
            Text = text;
            Timestamp = DateTime.UtcNow;
        }
    }
}
