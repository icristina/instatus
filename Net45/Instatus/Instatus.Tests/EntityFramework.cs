using System;
using System.Linq;
using System.Data.Entity;
using Instatus.Integration.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Instatus.Core;

namespace Instatus.Tests
{
    public class TestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    
    public class TestEntityModel : DbContext
    {
        public IDbSet<TestEntity> TestEntities { get; set; }
    }

    public class TestEntityModelInitializer : DropCreateDatabaseAlways<TestEntityModel>
    {
        private int count;
        
        protected override void Seed(TestEntityModel context)
        {
            for (var i = 0; i < 10; i++)
            {
                context.TestEntities.Add(new TestEntity()
                {
                    Name = i.ToString()
                });
            }
            
            base.Seed(context);
        }

        public TestEntityModelInitializer(int count)
        {
            this.count = count;
        }
    }
    
    [TestClass]
    public class EntityFramework
    {
        private IEntityStorage entityStorage = new EfEntityStorage<TestEntityModel>();
        private int count = 10;
        
        [TestInitialize]
        public void InitializeDatabase()
        {
            Database.SetInitializer<TestEntityModel>(new TestEntityModelInitializer(count));
        }
        
        [TestMethod]
        public void EntityStorageWrapper()
        {
            var testEntityCount = entityStorage.Set<TestEntity>().Count();

            Assert.AreEqual(10, count);
        }

        [TestMethod]
        public void FindEntity()
        {
            var testEntity = entityStorage.Set<TestEntity>().Find(1);

            Assert.AreEqual("0", testEntity.Name);
        }
    }
}
