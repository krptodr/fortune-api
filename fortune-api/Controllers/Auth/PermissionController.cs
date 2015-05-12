using fortune_api.Dtos.Auth;
using fortune_api.Persistence;
using fortune_api.Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace fortune_api.Controllers.Auth
{
    public class PermissionController : ApiController
    {
        private IPermissionService permissionService;
        private IUnitOfWork unitOfWork;

        public PermissionController(IPermissionService permissionService, IUnitOfWork unitOfWork)
        {
            this.permissionService = permissionService;
            this.unitOfWork = unitOfWork;
        }

        // GET api/permissions/{permissionId}
        [Route("api/permissions/{permissionId:guid}")]
        [HttpGet]
        public HttpResponseMessage Get(Guid permissionId)
        {
            PermissionDto dto = this.permissionService.Get(permissionId);
            return Request.CreateResponse(HttpStatusCode.OK, dto);
        }

        // GET api/permissions
        [Route("api/permissions")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            PermissionDto[] dtos = this.permissionService.Get();
            return Request.CreateResponse(HttpStatusCode.OK, dtos);
        }

        // PUT api/permissions
        [Route("api/permissions")]
        [HttpPut]
        public HttpResponseMessage Add([FromBody] PermissionDto dto)
        {
            dto = this.permissionService.Add(dto);
            this.unitOfWork.Save();
            return Request.CreateResponse(HttpStatusCode.OK, dto);
        }

        // DELETE api/permissions/{permissionId}
        [Route("api/permissions/{permissionId:guid}")]
        [HttpDelete]
        public HttpResponseMessage Delete(Guid permissionId)
        {
            this.permissionService.Delete(permissionId);
            this.unitOfWork.Save();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
