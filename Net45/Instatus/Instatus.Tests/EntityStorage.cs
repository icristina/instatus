using System;
using System.Linq;
using Instatus.Core.InMemory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instatus.Tests
{
    [TestClass]
    public class EntityStorage
    {
        public class User
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
        }

        public class Storage : InMemoryEntityStorage
        {
            public Storage()
            {
                var users = Set<User>();
                var user1 = users.Create();

                user1.Id = 1;
                user1.FirstName = "a";

                var user2 = users.Create();

                user2.Id = 2;
                user2.FirstName = "b";
            }
        }
        
        [TestMethod]
        public void Count()
        {
            var storage = new Storage();

            Assert.AreEqual(2, storage.Set<User>().Count());
        }

        [TestMethod]
        public void Find()
        {
            var storage = new Storage();

            Assert.AreEqual("b", storage.Set<User>().Find(2).FirstName);
        }
    }
}
