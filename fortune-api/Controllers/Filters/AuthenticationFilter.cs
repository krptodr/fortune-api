using fortune_api.Exceptions;
using fortune_api.Services.Auth;
using fortune_api.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using System.Security.Principal;
using fortune_api.Dtos.Auth;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace fortune_api.Controllers.Filters
{
    public class AuthenticationFilter : IAuthenticationFilter
    {
        private IJwtService jwtService;
        private IUserService userService;

        public AuthenticationFilter(IJwtService jwtService, IUserService userService)
        {
            Contract.Assert(jwtService != null);
            Contract.Assert(userService != null);
            this.jwtService = jwtService;
            this.userService = userService;
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {          
            // Look for credentials in request
            HttpRequestMessage req = context.Request;
            AuthenticationHeaderValue authHeader = req.Headers.Authorization;

            // If there are no credentials, do nothing
            if (authHeader == null)
            {
                return;
            }

            // If there are credentials, but the filter does not recognize the authentication scheme, do nothing
            if (authHeader.Scheme != "JWT")
            {
                return;
            }

            try
            {
                // Attempt to parse user id from auth header
                if (String.IsNullOrEmpty(authHeader.Parameter))
                {
                    throw new InvalidCredentialsException();
                }
                string jwt = authHeader.Parameter;
                Dictionary<string, string> jwtPayload = this.jwtService.ParseToken(jwt);
                Guid userId;
                try
                {
                    userId = Guid.Parse(jwtPayload["sub"]);
                }
                catch (Exception e)
                {
                    if (e is FormatException || e is OverflowException)
                    {
                        throw new InvalidCredentialsException();
                    }
                    else
                    {
                        throw e;
                    }
                }

                //Get user
                UserDto user;
                try
                {
                    user = this.userService.Get(userId);
                }
                catch (DoesNotExistException)
                {
                    throw new InvalidCredentialsException();
                }

                //Convert permissions to strings
                List<PermissionDto> permissions = user.Permissions;
                int numPermissions = permissions.Count();
                string[] permissionStrings = new string[numPermissions];
                for (int i = 0; i < numPermissions; i++)
                {
                    permissionStrings[i] = permissions[i].Name;
                }

                //Set principal
                context.Principal = new GenericPrincipal(new GenericIdentity(userId.ToString()), permissionStrings);
                return;
            }
            catch (InvalidCredentialsException e)
            {
                context.ErrorResult = new AuthenticationFailureResult(e.Message, req);
                return;
            }
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return;
        }

        public bool AllowMultiple
        {
            get { return false; }
        }
    }
}