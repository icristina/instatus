using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instatus.Tests
{
    [TestClass]
    public class Validation
    {
        public class ValidationViewModel
        {
            [Required]
            [RegularExpression(WebConstant.RegularExpression.EmailAddress)]
            public string EmailAddress { get; set; }

            [Required]
            public string Message { get; set; }
        }
        
        [TestMethod]
        public void InvalidModel()
        {
            var viewModel = new ValidationViewModel()
            {
                EmailAddress = "a",
                Message = string.Empty
            };

            Assert.IsFalse(viewModel.IsValid());
        }

        [TestMethod]
        public void ValidModel()
        {
            var viewModel = new ValidationViewModel()
            {
                EmailAddress = "a@b.com",
                Message = "a"
            };

            Assert.IsTrue(viewModel.IsValid());
        }
    }
}
