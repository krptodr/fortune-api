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
        private LoadBoardDbContext context;

        public ValuesController(LoadBoardDbContext context)
        {
            this.context = context;
        }

        // GET api/values
        public List<Value> Get()
        {
            return this.context.Values.ToList<Value>();
        }

        // GET api/values/{id}
        public Value Get(Guid id)
        {
            return this.context.Values.Find(id);
        }

        // POST api/values
        public void Post([FromBody] Value value)
        {
            value.Id = Guid.NewGuid();
            this.context.Values.Add(value);
            this.context.SaveChanges();
        }

        // PUT api/values/{id}
        public void Put(int id, [FromBody] Value value)
        {
            Value dbValue = this.context.Values.Find(id);
            dbValue.Name = value.Name;
            this.context.SaveChanges();
        }

        // DELETE api/values/{id}
        public void Delete(Guid id)
        {
            Value value = this.context.Values.Find(id);
            this.context.Values.Remove(value);
            this.context.SaveChanges();
        }
    }
}
