using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace fortune_api.Controllers.Filters
{
    public class AuthenticationRequiredAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext context)
        {
            if (!SkipAuthorization(context))
            {
                HttpRequestMessage req = context.Request;
                if (!req.GetRequestContext().Principal.Identity.IsAuthenticated)
                {
                    HandleUnauthorizedRequest(context);
                }
            }
            return;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext context)
        {
            HttpRequestMessage req = context.Request;
            HttpResponseMessage res = req.CreateErrorResponse(HttpStatusCode.Unauthorized, "You must be logged in to access this resource");
            context.Response = res;
        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            Contract.Assert(actionContext != null);

            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() ||
                   actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
    }
}