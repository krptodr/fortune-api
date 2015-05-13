using fortune_api.Controllers.Filters;
using fortune_api.Dtos.Auth;
using fortune_api.Persistence;
using fortune_api.Services.Auth;
using fortune_api.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace fortune_api.Controllers.Auth
{
    public class AuthController : ApiController
    {
        private IAuthService authService;
        private IUserService userService;
        private IUnitOfWork unitOfWork;

        public AuthController(IAuthService authService, IUserService userService, IUnitOfWork unitOfWork)
        {
            this.authService = authService;
            this.userService = userService;
            this.unitOfWork = unitOfWork;
        }

        // POST api/auth/email/login
        [Route("api/auth/email/login")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage LogInViaEmail([FromBody] LoginViaEmailReq req)
        {
            LoginRes dto = this.authService.LogInViaEmail(req.Email, req.Password);
                
            return Request.CreateResponse(HttpStatusCode.OK, dto);
        }

        // POST api/auth/email/register
        [Route("api/auth/email/register")]
        [HttpPost]
        [Permissions(Roles="EditUsers")]
        public HttpResponseMessage RegisterEmail([FromBody] RegisterEmailReq req)
        {
            LoginRes dto = null;
            if(req.NewUser) {
                UserDto user = this.userService.Add(req.User);
                dto = this.authService.RegisterEmail(user.Id, req.Email, req.Password);
            } else {
                dto = this.authService.RegisterEmail(req.UserId, req.Email, req.Password);
            }
            this.unitOfWork.Save();
            return Request.CreateResponse(HttpStatusCode.OK, dto);
        }
    }
}