using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class DebugLogger : ILogger
    {
        public void Log(Exception exception, IDictionary<string, string> properties)
        {
            Debug.WriteLine(exception.Message);
            Debug.WriteLine(exception.StackTrace);

            if (exception.InnerException != null)
                Debug.WriteLine(exception.InnerException.Message);

            if (properties != null)
            {
                foreach (var property in properties)
                    Debug.WriteLine("{0} = {1}", property.Key, property.Value);
            }
        }
    }
}
