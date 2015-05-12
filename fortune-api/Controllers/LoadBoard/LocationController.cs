using fortune_api.Controllers;
using fortune_api.LoadBoard.Dtos;
using fortune_api.Persistence;
using fortune_api.LoadBoard.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using fortune_api.Services.Security;

namespace fortune_api.LoadBoard.Controllers
{
    [RoutePrefix("api/locations")]
    public class LocationController : ApiController
    {
        private ILocationService locationService;
        private IUnitOfWork unitOfWork;

        public LocationController(ILocationService locationService, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.locationService = locationService;
        }

        // GET api/locations/{id}
        [HttpGet]
        [Route("{id:guid}")]
        public HttpResponseMessage Get(Guid id)
        {
            LocationDto resDto = this.locationService.Get(id);
            return Request.CreateResponse(HttpStatusCode.OK, resDto);
        }

         
        // GET api/locations
        // 
        // Optional params:
        // includeDeleted
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get(bool includeDeleted = false)
        {
            LocationDto[] resDtos = this.locationService.Get(includeDeleted);
            return Request.CreateResponse(HttpStatusCode.OK, resDtos);
        }

        // POST api/locations
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Add([FromBody] LocationDto dto)
        {
            LocationDto resDtos = this.locationService.Add(dto);
            this.unitOfWork.Save();
            return Request.CreateResponse(HttpStatusCode.OK, resDtos);
        }

        // PUT api/locations
        [HttpPut]
        [Route("")]
        public HttpResponseMessage Update([FromBody] LocationDto dto)
        {
            LocationDto resDtos = this.locationService.Update(dto);
            this.unitOfWork.Save();
            return Request.CreateResponse(HttpStatusCode.OK, resDtos);
        }

        // DELETE api/locations/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public HttpResponseMessage Delete(Guid id)
        {
            this.locationService.Delete(id);
            this.unitOfWork.Save();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}