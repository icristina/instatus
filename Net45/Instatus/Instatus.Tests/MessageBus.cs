using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Instatus.Core;
using Instatus.Core.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instatus.Tests
{
    [TestClass]
    public class MessageBus
    {
        public class State
        {
            public int StartedActions { get; set; }
            public int CompletedActions { get; set; }
            public int Concurrency { get; set; }
            public int MaximumConcurrency { get; set; }
            public int ActionTimeout { get; set; }

            public void SubscribeToStrings(string message)
            {
                StartedActions++;
                Concurrency++;
                MaximumConcurrency = Math.Max(MaximumConcurrency, Concurrency);
                Thread.Sleep(ActionTimeout);
                CompletedActions++;
                Concurrency--;
            }

            public void SubscribeToIntegers(int integer)
            {
                StartedActions++;
                Thread.Sleep(ActionTimeout);
                CompletedActions++;
            }

            private int errors = 0;

            public void SubscribeToStringsWithError(string message)
            {
                StartedActions++;

                if (errors < 2)
                {
                    errors++;
                    throw new Exception("Failed");
                }

                CompletedActions++;
            }
        }

        public class Logger : ILogger 
        {
            public int Counter = 0;

            public void Log(Exception exception, IDictionary<string, string> properties)
            {
                Counter++;
            }
        }

        public class BaseMessage
        {

        }

        public class SubClassMessage : BaseMessage
        {

        }

        [TestMethod]
        public void FindActions()
        {
            var state = new State();
            var messageBus = new InMemoryMessageBus(null, null);

            messageBus.Subscribe<string>(state.SubscribeToStrings);
            messageBus.Subscribe<string>(state.SubscribeToStringsWithError);
            messageBus.Subscribe<int>(state.SubscribeToIntegers);
            messageBus.Subscribe<BaseMessage>(m => { });
            messageBus.Subscribe<SubClassMessage>(m => { });

            Assert.AreEqual(2, messageBus.MatchActions("a").Count());
            Assert.AreEqual(1, messageBus.MatchActions(1).Count());
            Assert.AreEqual(2, messageBus.MatchActions(new SubClassMessage()).Count());
        }

        [TestMethod]
        public void Concurrency()
        {
            var state = new State()
            {
                ActionTimeout = 1000
            };

            var messageBus = new InMemoryMessageBus(null, null)
            {
                Delay = 10,
                Concurrency = 10
            };

            messageBus.Start();
            messageBus.Subscribe<string>(state.SubscribeToStrings);

            for (var i = 1; i < 100; i++)
            {
                messageBus.Publish("a");
            }

            Thread.Sleep(1000);

            messageBus.Stop();

            Assert.IsTrue(state.MaximumConcurrency > 1);
        }

        [TestMethod]
        public void CompleteAllActions()
        {
            var state = new State();
            var messageBus = new InMemoryMessageBus(null, null)
            {
                Delay = 0,
                Concurrency = 10
            };

            messageBus.Start();
            messageBus.Subscribe<string>(state.SubscribeToStrings);
            messageBus.Subscribe<int>(state.SubscribeToIntegers);

            var count = 10;

            for (var i = 0; i < count; i++)
            {
                messageBus.Publish("a");
                messageBus.Publish(1);
            }

            Thread.Sleep(1000);

            messageBus.Stop();

            Assert.AreEqual(count * 2, state.CompletedActions);
        }

        [TestMethod]
        public void RetryPolicy()
        {
            var state = new State();
            var logger = new Logger();
            var messageBus = new InMemoryMessageBus(null, logger)
            {
                Delay = 0,
                Retry = 10
            };

            messageBus.Start();
            messageBus.Subscribe<string>(state.SubscribeToStringsWithError);
            messageBus.Publish("a");
            messageBus.Publish("b");

            Thread.Sleep(1000);

            messageBus.Stop();

            Assert.AreEqual(4, state.StartedActions);
            Assert.AreEqual(2, state.CompletedActions);
            Assert.AreEqual(2, logger.Counter);
        }
    }
}
