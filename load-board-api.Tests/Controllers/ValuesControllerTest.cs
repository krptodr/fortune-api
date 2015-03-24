using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using load_board_api;
using load_board_api.Controllers;
using load_board_api.Models;
using load_board_api.Persistence;
using load_board_api.Tests.Test_Start;

namespace load_board_api.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        private ValuesController valuesController;

        public ValuesControllerTest()
        {
            this.valuesController = new ValuesController(TestUtil.UNIT_OF_WORK);
        }

        [TestMethod]
        public void Get()
        {
            // Act
            IEnumerable<Value> result = this.valuesController.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetById()
        {
            // Act
            Value result = this.valuesController.Get(TestUtil.VALUES.ElementAt(0).Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(TestUtil.VALUES.ElementAt(0).Name, result.Name);
        }

        [TestMethod]
        public void GetNonexistentById()
        {
            // Act
            Value result = this.valuesController.Get(Guid.Empty);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Post()
        {
            // Act
            this.valuesController.Post(new Value { Name = "TEST" });
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Put()
        {
            // Act
            this.valuesController.Put(Guid.Empty, new Value { Name = "TEST" });
        }

        [TestMethod]
        public void Delete()
        {
            // Act
            this.valuesController.Delete(Guid.Empty);
        }
    }
}
