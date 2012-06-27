using System;
using System.Linq;
using Instatus.Core.Impl;
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
                
                users.Add(new User()
                {
                    Id = 1,
                    FirstName = "a"
                });

                users.Add(new User()
                {
                    Id = 2,
                    FirstName = "b"
                });
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
