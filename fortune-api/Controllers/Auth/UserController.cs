using fortune_api.Controllers.Filters;
using fortune_api.Dtos.Auth;
using fortune_api.Persistence;
using fortune_api.Services.Auth;
using fortune_api.Services.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace fortune_api.Controllers.Auth
{
    public class UserController : ApiController
    {
        private IUserService userService;
        private IUnitOfWork unitOfWork;

        public UserController(IUnitOfWork unitOfWork, IUserService userService)
        {
            Contract.Assert(unitOfWork != null);
            Contract.Assert(userService != null);
            this.unitOfWork = unitOfWork;
            this.userService = userService;
        }

        // GET api/users/{userId}
        [Route("api/users/{userId:guid}")]
        [HttpGet]
        public HttpResponseMessage GetUser(Guid userId)
        {
            UserDto dto = this.userService.Get(userId);
            return Request.CreateResponse(HttpStatusCode.OK, dto);
        }

        // GET api/users
        [Route("api/users")]
        [HttpGet]
        public HttpResponseMessage GetUsers()
        {
            UserDto[] dtos = this.userService.Get();
            return Request.CreateResponse(HttpStatusCode.OK, dtos);
        }

        // POST api/users
        [Route("api/users")]
        [HttpPost]
        [Permissions(Roles="EditUsers")]
        public HttpResponseMessage UpdateUser([FromBody] UserDto dto)
        {
            dto = this.userService.Update(dto);
            this.unitOfWork.Save();
            return Request.CreateResponse(HttpStatusCode.OK, dto);
        }

        // DELETE api/users/{userId}
        [Route("api/users/{userId:guid}")]
        [HttpDelete]
        [Permissions(Roles="EditUsers")]
        public HttpResponseMessage DeleteUser(Guid userId)
        {
            this.userService.Delete(userId);
            this.unitOfWork.Save();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}