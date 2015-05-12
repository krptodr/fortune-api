using fortune_api.Dtos.Auth;
using fortune_api.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fortune_api.Services.Auth
{
    public interface IPermissionService
    {
        /// <summary>
        /// Get all permissions
        /// </summary>
        /// <returns>Permissions</returns>
        PermissionDto[] Get();

        /// <summary>
        /// Gets the permission with the specified id
        /// </summary>
        /// <param name="id">Permission Id</param>
        /// <returns>Permission</returns>
        PermissionDto Get(Guid id);

        /// <summary>
        /// Adds permission
        /// </summary>
        /// <param name="dto">Permission</param>
        /// <returns>Permission</returns>
        PermissionDto Add(PermissionDto dto);

        /// <summary>
        /// Hard-deletes the permission with the specified id
        /// </summary>
        /// <param name="id">Permission Id</param>
        void Delete(Guid id);
    }
}
