using load_board_api.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace load_board_api.Services
{
    public interface ITrailerService
    {
        /// <summary>
        /// Gets the trailer with the specified id
        /// </summary>
        /// <param name="id">Trailer Id</param>
        /// <returns>Trailer</returns>
        /// <exception cref="DoesNotExistException">Trailer does not exist</exception>
        TrailerDto Get(int id);

        /// <summary>
        /// Gets list of trailers
        /// </summary>
        /// <param name="includeDeleted">Optional - Whether to include deleted trailers</param>
        /// <param name="skip">Optional - Number of trailers to skip</param>
        /// <param name="keep">Optional - Number of trailers to include</param>
        /// <returns>List of trailers</returns>
        TrailerDto Get(bool includeDeleted = false, int skip = -1, int keep = -1);

        /// <summary>
        /// Adds trailer
        /// </summary>
        /// <param name="dto">Trailer to add</param>
        /// <returns>Saved trailer</returns>
        /// <exception cref="AlreadyExistsException">Trailer already exists</exception>
        TrailerDto Add(TrailerDto dto);

        /// <summary>
        /// Updates trailer
        /// </summary>
        /// <param name="dto">Trailer to update</param>
        /// <returns>Saved trailer</returns>
        /// <exception cref="DoesNotExistException">Trailer does not exist</exception>
        /// <exception cref="ConflictException">Conflict occurred</exception>
        TrailerDto Update(TrailerDto dto);

        /// <summary>
        /// Soft deletes trailer
        /// </summary>
        /// <param name="id">Trailer Id</param>
        void Delete(int id);
    }
}
