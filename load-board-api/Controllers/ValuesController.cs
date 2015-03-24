using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using load_board_api.Persistence;
using load_board_api.Models;

namespace load_board_api.Controllers
{
    public class ValuesController : ApiController
    {
        private IUnitOfWork unitOfWork;

        public ValuesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET api/values
        public List<Value> Get()
        {
            return unitOfWork.ValueRepo.Get(filter: x => true).ToList<Value>();
        }

        // GET api/values/{id}
        public Value Get(Guid id)
        {
            return unitOfWork.ValueRepo.Get(id);
        }

        // POST api/values
        public void Post([FromBody] Value value)
        {
            value.Id = Guid.NewGuid();
            this.unitOfWork.ValueRepo.Insert(value);
            this.unitOfWork.Save();
        }

        // PUT api/values/{id}
        public void Put(Guid id, [FromBody] Value value)
        {
            Value dbValue = this.unitOfWork.ValueRepo.Get(id);
            dbValue.Name = value.Name;
            this.unitOfWork.ValueRepo.Update(value);
            this.unitOfWork.Save();
        }

        // DELETE api/values/{id}
        public void Delete(Guid id)
        {
            this.unitOfWork.ValueRepo.Delete(id);
            this.unitOfWork.Save();
        }
    }
}
