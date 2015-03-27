﻿using load_board_api.Dtos;
using load_board_api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace load_board_api.Controllers
{
    [RoutePrefix("api/loads")]
    public class LoadController : AbstractController
    {
        private ILoadService loadService;

        public LoadController(ILoadService loadService)
        {
            this.loadService = loadService;
        }

        // GET api/loads/{id}
        [Route("{id:guid}")]
        [HttpGet]
        public HttpResponseMessage Get(Guid id)
        {
            HttpResponseMessage res = null;

            try
            {
                LoadDto resDto = this.loadService.Get(id);
                res = Request.CreateResponse(HttpStatusCode.OK, resDto);
            }
            catch (Exception e)
            {
                res = GetErrorResponse(e);
            }

            return res;
        }

        // GET api/loads
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
                LoadDto[] resDtos = this.loadService.Get(includeDeleted, skip, num);
                res = Request.CreateResponse(HttpStatusCode.OK, resDtos);
            }
            catch (Exception e)
            {
                res = GetErrorResponse(e);
            }

            return res;
        }

        // POST api/loads
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Add([FromBody] LoadDto dto)
        {
            HttpResponseMessage res = null;

            try
            {
                LoadDto resDtos = this.loadService.Add(dto);
                res = Request.CreateResponse(HttpStatusCode.OK, resDtos);
            }
            catch (Exception e)
            {
                res = GetErrorResponse(e);
            }

            return res;
        }

        // PUT api/loads
        [HttpPut]
        [Route("")]
        public HttpResponseMessage Update([FromBody] LoadDto dto)
        {
            HttpResponseMessage res = null;

            try
            {
                LoadDto resDtos = this.loadService.Update(dto);
                res = Request.CreateResponse(HttpStatusCode.OK, resDtos);
            }
            catch (Exception e)
            {
                res = GetErrorResponse(e);
            }

            return res;
        }

        // DELETE api/loads/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public HttpResponseMessage Delete(Guid id)
        {
            HttpResponseMessage res = null;

            try
            {
                this.loadService.Delete(id);
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
