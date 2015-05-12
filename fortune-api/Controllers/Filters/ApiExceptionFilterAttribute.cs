using fortune_api.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace fortune_api.Controllers.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private static readonly string ENV = ConfigurationManager.AppSettings["ENV"];

        public override void OnException(HttpActionExecutedContext context)
        {
            HttpRequestMessage req = context.ActionContext.Request;

            HttpResponseMessage res = GetErrorResponse(req, context.Exception);

            context.Response = res;
        }

        private HttpResponseMessage GetErrorResponse(HttpRequestMessage req, Exception e)
        {
            HttpResponseMessage res = null;
            HttpStatusCode statusCode = GetHttpStatusCode(e);
            if (ENV == "PROD" && statusCode == HttpStatusCode.InternalServerError)
            {
                res = req.CreateResponse(statusCode);
            }
            else
            {
                res = req.CreateErrorResponse(statusCode, e.GetBaseException().Message);
            }
            return res;
        }

        private HttpStatusCode GetHttpStatusCode(Exception e)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            if (e is AlreadyExistsException)
            {
                statusCode = HttpStatusCode.Forbidden;
            }
            else if (e is DoesNotExistException)
            {
                statusCode = HttpStatusCode.NotFound;
            }
            else if (e is ConflictException)
            {
                statusCode = HttpStatusCode.Conflict;
            }
            else if (e is UnauthorizedException || e is InvalidCredentialsException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            else
            {
                Trace.TraceError(e.GetBaseException().Message);
            }

            return statusCode;
        }
    }
}