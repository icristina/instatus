using System;
using Instatus.Integration.AutoMapper;
using Instatus.Integration.ValueInjecter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instatus.Tests
{
    [TestClass]
    public class Mapper
    {
        public class ViewModel
        {
            public string Title { get; set; }
            public int SortOrder { get; set; }
        }

        public class Entity
        {
            public string Title { get; set; }
            public int SortOrder { get; set; }
        }

        [TestMethod]
        public void Map()
        {           
            var mapper = new ValueInjecterMapper();
            var viewModel = new ViewModel()
            {
                Title = "a",
                SortOrder = 2
            };
            var entity = mapper.Map<Entity>(viewModel);

            Assert.AreEqual("a", entity.Title);
        }

        [TestMethod]
        public void Merge()
        {
            var mapper = new ValueInjecterMapper();
            var viewModel = new ViewModel()
            {
                Title = "a",
                SortOrder = 2
            };
            var entity = new Entity()
            {
                Title = "b",
                SortOrder = 3
            };

            mapper.Inject(entity, viewModel);

            Assert.AreEqual("a", entity.Title);
            Assert.AreEqual(2, entity.SortOrder);
        }
    }
}
