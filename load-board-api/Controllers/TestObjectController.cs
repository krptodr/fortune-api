using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using load_board_api.Persistence;
using load_board_api.Models;
using System.Diagnostics;
using System.Configuration;

namespace load_board_api.Controllers
{
    [RoutePrefix("api/testobjects")]
    public class TestObjectController : ApiController
    {
        private IUnitOfWork unitOfWork;

        public TestObjectController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET api/testobjects/hidden
        [HttpGet]
        [Route("hidden")]
        public string GetHidden()
        {
            try
            {
                return ConfigurationManager.AppSettings["TEST"];
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                throw;
            }
        }

        // GET api/testobjects
        [HttpGet]
        [Route("")]
        public List<TestObject> Get()
        {
            try
            {
                return unitOfWork.TestObjectRepo.Get(filter: x => true).ToList<TestObject>();
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                throw;
            }
        }

        // GET api/testobjects/{id}
        [HttpGet]
        [Route("{id:guid}")]
        public TestObject Get(Guid id)
        {
            return unitOfWork.TestObjectRepo.Get(id);
        }

        // POST api/testobjects
        [HttpPost]
        [Route("")]
        public void Post([FromBody] TestObject value)
        {
            value.Id = Guid.NewGuid();
            this.unitOfWork.TestObjectRepo.Insert(value);
            this.unitOfWork.Save();
        }

        // PUT api/testobjects/{id}
        [HttpPut]
        [Route("{id:guid}")]
        public void Put(Guid id, [FromBody] TestObject value)
        {
            TestObject dbValue = this.unitOfWork.TestObjectRepo.Get(id);
            dbValue.Name = value.Name;
            this.unitOfWork.TestObjectRepo.Update(value);
            this.unitOfWork.Save();
        }

        // DELETE api/values/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public void Delete(Guid id)
        {
            this.unitOfWork.TestObjectRepo.Delete(id);
            this.unitOfWork.Save();
        }
    }
}
