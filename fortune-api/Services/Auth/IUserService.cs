using fortune_api.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fortune_api.Services.Auth
{
    public interface IUserService
    {
        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>List of users</returns>
        UserDto[] Get();

        /// <summary>
        /// Gets the user with the specified id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>User</returns>
        UserDto Get(Guid id);

        /// <summary>
        /// Adds user
        /// </summary>
        /// <param name="userDto">User</param>
        /// <returns>User</returns>
        UserDto Add(UserDto userDto);

        /// <summary>
        /// Updates user
        /// </summary>
        /// <param name="userDto">User</param>
        /// <returns>User</returns>
        UserDto Update(UserDto userDto);

        /// <summary>
        /// Deletes user
        /// </summary>
        /// <param name="id">User Id</param>
        void Delete(Guid id);
    }
}
