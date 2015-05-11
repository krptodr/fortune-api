using fortune_api.LoadBoard.Dtos;
using fortune_api.LoadBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fortune_api.LoadBoard.Services
{
    public interface ILocationService
    {
        /// <summary>
        /// Gets the location with the specified id
        /// </summary>
        /// <param name="id">Location Id</param>
        /// <returns>Location</returns>
        /// <exception cref="DoesNotExistException">Location does not exist</exception>
        LocationDto Get(Guid id);

        /// <summary>
        /// Gets all locations
        /// </summary>
        /// <param name="includeDeleted">Optional - Whether to include deleted locations</param>
        /// <returns>List of locations</returns>
        LocationDto[] Get(bool includeDeleted = false);

        /// <summary>
        /// Adds location
        /// </summary>
        /// <param name="location">Location to add</param>
        /// <returns>Saved location</returns>
        /// <exception cref="AlreadyExistsException">A location with the same name already exists</exception>
        LocationDto Add(LocationDto dto);

        /// <summary>
        /// Updates location
        /// </summary>
        /// <param name="location">Location to update</param>
        /// <returns>Saved location</returns>
        /// <exception cref="DoesNotExistException">Location does not exist</exception>
        /// <exception cref="ConflictException">Conflict occurred</exception>
        LocationDto Update(LocationDto dto);

        /// <summary>
        /// Soft deletes location
        /// </summary>
        /// <param name="id">Location Id</param>
        void Delete(Guid id);
    }
}
