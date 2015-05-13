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
    [RoutePrefix("api/loads")]
    public class LoadController : ApiController
    {
        private ILoadService loadService;
        private IUnitOfWork unitOfWork;

        public LoadController(ILoadService loadService, IUnitOfWork unitOfWork)
        {
            Contract.Assert(loadService != null);
            Contract.Assert(unitOfWork != null);
            this.unitOfWork = unitOfWork;
            this.loadService = loadService;
        }

        // GET api/loads/{id}
        [Route("{id:guid}")]
        [HttpGet]
        [Permissions(Roles="ViewLoads")]
        public HttpResponseMessage Get(Guid id)
        {
            LoadDto resDto = this.loadService.Get(id);
            return Request.CreateResponse(HttpStatusCode.OK, resDto);
        }

        // GET api/loads
        // 
        // Optional params:
        // includeDeleted
        // skip
        // num
        [HttpGet]
        [Route("")]
        [Permissions(Roles="ViewLoads")]
        public HttpResponseMessage Get(bool includeDeleted = false, int skip = -1, int num = -1)
        {
            LoadDto[] resDtos = this.loadService.Get(includeDeleted, skip, num);
            return Request.CreateResponse(HttpStatusCode.OK, resDtos);
        }

        // PUT api/loads
        [HttpPut]
        [Route("")]
        [Permissions(Roles="EditLoads")]
        public HttpResponseMessage Add([FromBody] LoadDto dto)
        {
            LoadDto resDtos = this.loadService.Add(dto);
            this.unitOfWork.Save();
            return Request.CreateResponse(HttpStatusCode.OK, resDtos);
        }

        // POST api/loads
        [HttpPost]
        [Route("")]
        [Permissions(Roles="EditLoads")]
        public HttpResponseMessage Update([FromBody] LoadDto dto)
        {
            LoadDto resDtos = this.loadService.Update(dto);
            this.unitOfWork.Save();
            return Request.CreateResponse(HttpStatusCode.OK, resDtos);
        }

        // DELETE api/loads/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        [Permissions(Roles="EditLoads")]
        public HttpResponseMessage Delete(Guid id)
        {
            this.loadService.Delete(id);
            this.unitOfWork.Save();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
