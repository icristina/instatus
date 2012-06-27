using System;
using System.Collections.Generic;
using Instatus.Core.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instatus.Tests
{
    [TestClass]
    public class Diagnostics
    {
        [TestMethod]
        public void Exception()
        {
            var exception = new Exception("a");
            var debugLogger = new DebugLogger();

            debugLogger.Log(exception, new Dictionary<string, string>()
            {
                { "Uri", "b" }
            });
        }        
        
        [TestMethod]
        public void Profiler()
        {
            var debugProfiler = new DebugProfiler();

            using (debugProfiler.Step("a"))
            {

            }
        }
    }
}
