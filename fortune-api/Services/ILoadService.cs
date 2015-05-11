using fortune_api.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fortune_api.Services
{
    public interface ILoadService
    {
        /// <summary>
        /// Gets the load with the specified id
        /// </summary>
        /// <param name="id">Load Id</param>
        /// <returns>Load</returns>
        /// <exception cref="DoesNotExistException">Load does not exist</exception>
        LoadDto Get(Guid id);

        /// <summary>
        /// Gets list of loads
        /// </summary>
        /// <param name="includeDeleted">Optional - Whether to include deleted loads</param>
        /// <param name="skip">Optional - Number of loads to skip</param>
        /// <param name="num">Optional - Number of loads to include</param>
        /// <returns>List of loads</returns>
        LoadDto[] Get(bool includeDeleted = false, int skip = -1, int num = -1);

        /// <summary>
        /// Adds load
        /// </summary>
        /// <param name="dto">Load to add</param>
        /// <returns>Saved load</returns>
        /// <exception cref="AlreadyExistsException">Load already exists</exception>
        /// <exception cref="DoesNotExistException">Trailer, trailer location, origin, or destination do not exist</exception>
        LoadDto Add(LoadDto dto);

        /// <summary>
        /// Updates load
        /// </summary>
        /// <param name="dto">Load to update</param>
        /// <returns>Saved load</returns>
        /// <exception cref="DoesNotExistException">Load, trailer, trailer location, origin, or destination do not</exception>
        /// <exception cref="ConflictException">Conflict occurred</exception>
        LoadDto Update(LoadDto dto);

        /// <summary>
        /// Deletes load
        /// </summary>
        /// <param name="id">Load to delete</param>
        void Delete(Guid id);
    }
}
