using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace fortune_api.Controllers.Filters
{
    public class PermissionsAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext context)
        {
            HttpRequestMessage req = context.Request;
            GenericPrincipal principal = req.GetRequestContext().Principal as GenericPrincipal;
            string[] roles = this.Roles.Split(',');
            int numRoles = roles.Length;
            bool hasPermissions = true;
            List<string> missingPermissions = new List<string>();
            foreach(string role in roles)
            {
                if(!principal.IsInRole(role)) {
                    hasPermissions = false;
                    missingPermissions.Add(role);
                }
            }
            if (!hasPermissions) 
            {
                HandleUnauthorizedRequest(context, missingPermissions);
            }
        }

        public void HandleUnauthorizedRequest(HttpActionContext context, List<string> missingPermissions)
        {
            HttpRequestMessage req = context.Request;

            StringBuilder sb = new StringBuilder();
            sb.Append("Missing permissions: ");
            foreach (string permission in missingPermissions)
            {
                sb.Append(permission);
                sb.Append(", ");
            }
            sb.Remove(sb.Length - 2, 2);

            HttpResponseMessage res = req.CreateErrorResponse(HttpStatusCode.Unauthorized, sb.ToString());
            context.Response = res;
        }
    }
}