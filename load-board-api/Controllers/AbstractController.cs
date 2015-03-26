using load_board_api.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace load_board_api.Controllers
{
    public abstract class AbstractController : ApiController
    {
        public HttpStatusCode GetHttpStatusCode(Exception e)
        {
            string env = ConfigurationManager.AppSettings["ENV"];

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
            else if (env == "TEST" || env == "DEV")
            {
                Trace.TraceError(e.GetBaseException().Message);
            }

            return statusCode;
        }
    }
}