using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Instatus.Core.Impl;
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

        [TestMethod]
        public void FlushQueue()
        {
            var flushCount = 0;
            var concurrantBag = new ConcurrentBag<string>();
            var flushAction = new Action<List<string>>((s) => {
                Interlocked.Increment(ref flushCount);
                
                if (flushCount == 2)
                    throw new Exception(); // allow exception
                
                foreach (var st in s)
                    concurrantBag.Add(st);
            });
            
            var inMemoryQueue = new InMemoryQueue<string>(2, flushAction);

            inMemoryQueue.Enqueue("a");
            inMemoryQueue.Enqueue("b");
            inMemoryQueue.Enqueue("c");
            inMemoryQueue.Enqueue("d");
            inMemoryQueue.Enqueue("e");
            inMemoryQueue.Enqueue("f");
            inMemoryQueue.Enqueue("g");
            inMemoryQueue.Enqueue("h");
            inMemoryQueue.Enqueue("i");

            Thread.Sleep(200);

            Assert.AreEqual(4, flushCount);
            Assert.AreEqual(6, concurrantBag.Distinct().Count());
        }
    }
}
