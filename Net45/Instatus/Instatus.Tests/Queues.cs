using System;
using System.Linq;
using Instatus.Core.InMemory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instatus.Tests
{
    [TestClass]
    public class Queues
    {
        [TestMethod]
        public void NoLimit()
        {
            var inMemoryQueue = new InMemoryQueue<string>();

            inMemoryQueue.Enqueue("a");
            inMemoryQueue.Enqueue("b");
            inMemoryQueue.Enqueue("c");

            Assert.AreEqual(3, inMemoryQueue.Items.Count());

            string first;

            inMemoryQueue.TryDequeue(out first);

            Assert.AreEqual("a", first);
        }        
        
        [TestMethod]
        public void Limit()
        {
            var inMemoryQueue = new InMemoryQueue<string>(2);

            inMemoryQueue.Enqueue("a");
            inMemoryQueue.Enqueue("b");
            inMemoryQueue.Enqueue("c");

            Assert.AreEqual(2, inMemoryQueue.Items.Count());
        }

        [TestMethod]
        public void FirstInFirstOut()
        {
            var inMemoryQueue = new InMemoryQueue<string>(2);

            inMemoryQueue.Enqueue("a");
            inMemoryQueue.Enqueue("b");
            inMemoryQueue.Enqueue("c");

            string item;

            inMemoryQueue.TryDequeue(out item);

            Assert.AreEqual("b", item);

            inMemoryQueue.TryDequeue(out item);

            Assert.AreEqual("c", item);
        }
    }
}
