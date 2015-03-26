using load_board_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace load_board_api.Services
{
    public interface ILocationService
    {
        /// <summary>
        /// Gets the location with the specified id
        /// </summary>
        /// <param name="id">Location Id</param>
        /// <returns>Location</returns>
        /// <exception cref="DoesNotExistException">Location does not exist</exception>
        Location Get(Guid id);

        /// <summary>
        /// Gets all locations that have not been deleted
        /// </summary>
        /// <returns>List of locations</returns>
        Location[] Get();

        /// <summary>
        /// Gets all locations
        /// </summary>
        /// <param name="includeDeleted">Whether to include deleted locations</param>
        /// <returns>List of locations</returns>
        Location[] Get(bool includeDeleted);

        /// <summary>
        /// Adds location
        /// </summary>
        /// <param name="location">Location to add</param>
        /// <returns>Saved location</returns>
        /// <exception cref="AlreadyExistsException">A location with the same name already exists</exception>
        Location Add(Location location);

        /// <summary>
        /// Updates location
        /// </summary>
        /// <param name="location">Location to update</param>
        /// <returns>Saved location</returns>
        /// <exception cref="DoesNotExistException">Location does not exist</exception>
        /// <exception cref="OutdatedDataException">Location data is outdated</exception>
        Location Update(Location location);

        /// <summary>
        /// Soft deletes location
        /// </summary>
        /// <param name="id">Location Id</param>
        void Delete(Guid id);
    }
}
