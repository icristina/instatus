using System;
using System.Threading;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Instatus;
using Instatus.Data;
using Instatus.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instatus.Tests
{
    [TestClass]
    public class MessageQueue
    {
        private static IMessageQueue<MessageProcessingRequest> messageQueue = new InMemoryMessageQueue<MessageProcessingRequest>();
        
        [TestMethod]
        public void Background()
        {
            var messageCount = 6;
            var builder = new ContainerBuilder();

            builder.RegisterInstance(messageQueue).As<IMessageQueue<MessageProcessingRequest>>().ExternallyOwned();
            builder.RegisterType<MessageProcessingJob>().As<IJob<MessageProcessingRequest, bool>>().InstancePerDependency();
            builder.RegisterType<InMemoryLoggingService>().As<ILoggingService>().SingleInstance();

            var dependencyResolver = new AutofacDependencyResolver(builder.Build());

            Startup.RegisterWorker<MessageProcessingRequest>(delay: 1, parallelism: messageCount, lifetimeScope: dependencyResolver.ApplicationContainer);
            
            for(var i = 0; i < messageCount; i++)
                messageQueue.Enqueue(new MessageProcessingRequest()
                {
                    State = "1234"
                });

            Thread.Sleep(100);
            Assert.IsTrue(MessageProcessingJob.MaxConcurrancy > 1);
        }
    }

    public class MessageProcessingRequest
    {
        public string State { get; set; }
    }

    public class MessageProcessingJob : IJob<MessageProcessingRequest, bool>
    {
        public static int Concurrancy = 0;
        public static int MaxConcurrancy = 0;        
        
        public bool Process(MessageProcessingRequest context)
        {
            Concurrancy++;
            MaxConcurrancy = Math.Max(Concurrancy, MaxConcurrancy);
            Thread.Sleep(10);
            Concurrancy--;

            return true;
        }
    }
}
