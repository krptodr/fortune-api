using load_board_api.Dtos;
using load_board_api.Persistence;
using load_board_api.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace load_board_api.Controllers
{
    [RoutePrefix("api/locations")]
    public class LocationController : AbstractController
    {
        private ILocationService locationService;

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
            HttpResponseMessage res = null;

            try
            {
                LocationDto resDto = this.locationService.Get(id);
                res = Request.CreateResponse(HttpStatusCode.OK, resDto);
            }
            catch (Exception e)
            {
                res = GetErrorResponse(e);
            }

            return res;
        }

         
        // GET api/locations
        // 
        // Optional params:
        // includeDeleted
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get(bool includeDeleted = false)
        {
            HttpResponseMessage res = null;

            try
            {
                LocationDto[] resDtos = this.locationService.Get(includeDeleted);
                res = Request.CreateResponse(HttpStatusCode.OK, resDtos);
            }
            catch (Exception e)
            {
                res = GetErrorResponse(e);
            }

            return res;
        }

        // POST api/locations
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Add([FromBody] LocationDto dto)
        {
            HttpResponseMessage res = null;

            try
            {
                LocationDto resDtos = this.locationService.Add(dto);
                this.unitOfWork.Save();
                res = Request.CreateResponse(HttpStatusCode.OK, resDtos);
            }
            catch (Exception e)
            {
                res = GetErrorResponse(e);
            }

            return res;
        }

        // PUT api/locations
        [HttpPut]
        [Route("")]
        public HttpResponseMessage Update([FromBody] LocationDto dto)
        {
            HttpResponseMessage res = null;

            try
            {
                LocationDto resDtos = this.locationService.Update(dto);
                this.unitOfWork.Save();
                res = Request.CreateResponse(HttpStatusCode.OK, resDtos);
            }
            catch (Exception e)
            {
                res = GetErrorResponse(e);
            }

            return res;
        }

        // DELETE api/locations/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public HttpResponseMessage Delete(Guid id)
        {
            HttpResponseMessage res = null;

            try
            {
                this.locationService.Delete(id);
                this.unitOfWork.Save();
                res = Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                res = GetErrorResponse(e);
            }

            return res;
        }
    }
}