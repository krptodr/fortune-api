using fortune_api.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fortune_api.Services.Auth
{
    public interface IAuthService
    {
        /// <summary>
        /// Attempts to log in via email & password
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Password</param>
        /// <returns>Login Response</returns>
        /// <exception cref="InvalidCredentialsException">Invalid email and/or password</exception>
        LoginRes LogInViaEmail(string email, string password);

        /// <summary>
        /// Registers email and password for the specified user
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="email">Email</param>
        /// <param name="password">Password</param>
        /// <returns>Login Response</returns>
        /// <exception cref="AlreadyExistsException">Email has already been registered</exception>
        /// <exception cref="DoesNotExistException">User with specified id does not exist</exception>
        LoginRes RegisterEmail(Guid userId, string email, string password);

        /// <summary>
        /// Hard-deletes all auth methods for the specified user
        /// </summary>
        /// <param name="userId">User Id</param>
        void RemoveUsersAuthMethods(Guid userId);
    }
}
