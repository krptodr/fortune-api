using fortune_api.Controllers;
using fortune_api.LoadBoard.Dtos;
using fortune_api.Persistence;
using fortune_api.LoadBoard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using fortune_api.Services.Security;

namespace fortune_api.LoadBoard.Controllers
{
    [RoutePrefix("api/trailers")]
    public class TrailerController : ApiController
    {
        private ITrailerService trailerService;
        private IUnitOfWork unitOfWork;

        public TrailerController(ITrailerService trailerService, IUnitOfWork unitOfWork)
        {
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

        // POST api/trailers
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Add([FromBody] TrailerDto dto)
        {
            TrailerDto resDtos = this.trailerService.Add(dto);
            this.unitOfWork.Save();
            return Request.CreateResponse(HttpStatusCode.OK, resDtos);
        }

        // PUT api/trailers
        [HttpPut]
        [Route("")]
        public HttpResponseMessage Update([FromBody] TrailerDto dto)
        {
            TrailerDto resDtos = this.trailerService.Update(dto);
            this.unitOfWork.Save();
            return Request.CreateResponse(HttpStatusCode.OK, resDtos);
        }

        // DELETE api/trailers/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public HttpResponseMessage Delete(int id)
        {
            this.trailerService.Delete(id);
            this.unitOfWork.Save();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
