using load_board_api.Dtos;
using load_board_api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace load_board_api.Controllers
{
    [RoutePrefix("api/trailers")]
    public class TrailerController : AbstractController
    {
        private ITrailerService trailerService;

        public TrailerController(ITrailerService trailerService)
        {
            this.trailerService = trailerService;
        }

        // GET api/trailers/{id}
        [Route("{id:int}")]
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            HttpResponseMessage res = null;

            try
            {
                TrailerDto resDto = this.trailerService.Get(id);
                res = Request.CreateResponse(HttpStatusCode.OK, resDto);
            }
            catch (Exception e)
            {
                res = GetErrorResponse(e);
            }

            return res;
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
            HttpResponseMessage res = null;

            try
            {
                TrailerDto[] resDtos = this.trailerService.Get(includeDeleted, skip, num);
                res = Request.CreateResponse(HttpStatusCode.OK, resDtos);
            }
            catch (Exception e)
            {
                res = GetErrorResponse(e);
            }

            return res;
        }

        // POST api/trailers
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Add([FromBody] TrailerDto dto)
        {
            HttpResponseMessage res = null;

            try
            {
                TrailerDto resDtos = this.trailerService.Add(dto);
                res = Request.CreateResponse(HttpStatusCode.OK, resDtos);
            }
            catch (Exception e)
            {
                res = GetErrorResponse(e);
            }

            return res;
        }

        // PUT api/trailers
        [HttpPut]
        [Route("")]
        public HttpResponseMessage Update([FromBody] TrailerDto dto)
        {
            HttpResponseMessage res = null;

            try
            {
                TrailerDto resDtos = this.trailerService.Update(dto);
                res = Request.CreateResponse(HttpStatusCode.OK, resDtos);
            }
            catch (Exception e)
            {
                res = GetErrorResponse(e);
            }

            return res;
        }

        // DELETE api/trailers/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage res = null;

            try
            {
                this.trailerService.Delete(id);
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
