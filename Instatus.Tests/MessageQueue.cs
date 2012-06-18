using System;
using System.Threading;
using Instatus;
using Instatus.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instatus.Tests
{
    [TestClass]
    public class MessageQueue
    {
        private static int Concurrancy = 0;
        private static int MaxConcurrancy = 0;
        
        [TestMethod]
        public void Background()
        {
            var queue = new InMemoryMessageQueue<string>();
            var messageCount = 6;

            queue.RegisterBackgroundHandler(ProcessString, 0, 0, messageCount);
            
            for(var i = 0; i < messageCount; i++) 
                queue.Enqueue("Start");

            Thread.Sleep(50);
            Assert.IsTrue(MaxConcurrancy > (messageCount / 2)); // want concurrancy of at least half the message count
        }

        public void ProcessString(string value)
        {
            Concurrancy++;
            MaxConcurrancy = Math.Max(Concurrancy, MaxConcurrancy);
            Thread.Sleep(10);
            Concurrancy--;
        }
    }
}
