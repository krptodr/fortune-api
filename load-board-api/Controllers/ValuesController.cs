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
        // GET api/values
        public List<Value> Get()
        {
            LoadBoardDbContext context = new LoadBoardDbContext();
            return context.Values.ToList<Value>();
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
