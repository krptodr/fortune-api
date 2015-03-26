using load_board_api.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace load_board_api.Controllers
{
    public abstract class AbstractController : ApiController
    {
        private static readonly string ENV = ConfigurationManager.AppSettings["ENV"]; 

        public HttpStatusCode GetHttpStatusCode(Exception e)
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
            else
            {
                Trace.TraceError(e.GetBaseException().Message);
            }

            return statusCode;
        }

        public HttpResponseMessage GetErrorResponse(Exception e)
        {
            HttpResponseMessage res = null;
            HttpStatusCode statusCode = GetHttpStatusCode(e);
            if (ENV == "PROD" && statusCode == HttpStatusCode.InternalServerError)
            {
                res = Request.CreateResponse(statusCode);
            }
            else
            {
                res = Request.CreateErrorResponse(statusCode, e.GetBaseException().Message);
            }
            return res;
        }
    }
}