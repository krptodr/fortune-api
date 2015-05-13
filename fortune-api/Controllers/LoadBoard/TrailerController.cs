using fortune_api.Controllers;
using fortune_api.Dtos.LoadBoard;
using fortune_api.Persistence;
using fortune_api.Services.LoadBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using fortune_api.Services.Security;
using fortune_api.Controllers.Filters;
using System.Diagnostics.Contracts;

namespace fortune_api.Controllers.LoadBoard
{
    [RoutePrefix("api/trailers")]
    public class TrailerController : ApiController
    {
        private ITrailerService trailerService;
        private IUnitOfWork unitOfWork;

        public TrailerController(ITrailerService trailerService, IUnitOfWork unitOfWork)
        {
            Contract.Assert(trailerService != null);
            Contract.Assert(unitOfWork != null);
            this.unitOfWork = unitOfWork;
            this.trailerService = trailerService;
        }

        // GET api/trailers/{id}
        [Route("{id:int}")]
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            TrailerDto resDto = this.trailerService.Get(id);
            return Request.CreateResponse(HttpStatusCode.OK, resDto);
        }

        // GET api/trailers
        // 
        // Optional params:
        // includeDeleted
        // skip
        // num
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get(bool includeDeleted = false, int skip = -1, int num = -1)
        {
            TrailerDto[] resDtos = this.trailerService.Get(includeDeleted, skip, num);
            return Request.CreateResponse(HttpStatusCode.OK, resDtos);
        }

        // PUT api/trailers
        [HttpPut]
        [Route("")]
        [Permissions(Roles="EditTrailers")]
        public HttpResponseMessage Add([FromBody] TrailerDto dto)
        {
            TrailerDto resDtos = this.trailerService.Add(dto);
            this.unitOfWork.Save();
            return Request.CreateResponse(HttpStatusCode.OK, resDtos);
        }

        // POST api/trailers
        [HttpPost]
        [Route("")]
        [Permissions(Roles = "EditTrailers")]
        public HttpResponseMessage Update([FromBody] TrailerDto dto)
        {
            TrailerDto resDtos = this.trailerService.Update(dto);
            this.unitOfWork.Save();
            return Request.CreateResponse(HttpStatusCode.OK, resDtos);
        }

        // DELETE api/trailers/{id}
        [HttpDelete]
        [Route("{id:int}")]
        [Permissions(Roles = "EditTrailers")]
        public HttpResponseMessage Delete(int id)
        {
            this.trailerService.Delete(id);
            this.unitOfWork.Save();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
