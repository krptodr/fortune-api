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

namespace load_board_api.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        private ValuesController valuesController;

        public ValuesControllerTest()
        {
            LoadBoardDbContext context = new LoadBoardDbContext();
            IRepo<Value> valueRepo = new Repo<Value>(context);
            IUnitOfWork unitOfWork = new UnitOfWork(context, valueRepo);
            this.valuesController = new ValuesController(unitOfWork);
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
        public void GetNonexistentById()
        {
            // Act
            Value result = this.valuesController.Get(Guid.Empty);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete()
        {
            // Act
            this.valuesController.Delete(Guid.Empty);
        }
    }
}
