using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace fortune_api.Persistence
{
    /// <summary>
    /// Represents a generic repository for a specific entity type
    /// </summary>
    /// <typeparam name="TEntity">The type of entity stored in the repository</typeparam>
    public interface IRepo<TEntity> where TEntity : class {

        /// <summary>
        /// Determines if an entity with the given id exists
        /// </summary>
        /// <param name="id">Entity Id</param>
        /// <returns>Boolean value indicating whether the entity exists</returns>
        bool Exists(object id);

        /// <summary>
        /// Gets matching entities
        /// </summary>
        /// <param name="filter">Filter condition</param>
        /// <param name="skip">The number of entities to skip</param>
        /// <param name="num">The number of entities to return</param>
        /// <param name="orderBy">Column to order results by</param>
        /// <param name="includeProperties">Properties to eager-load</param>
        /// <returns>Matching entities</returns>
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            int skip = -1,
            int num = -1,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = ""
        );

        /// <summary>
        /// Gets the entity with the specified id
        /// </summary>
        /// <param name="id">Entity Id</param>
        /// <returns>Entity</returns>
        TEntity Get(object id);

        /// <summary>
        /// Adds the given entity
        /// </summary>
        /// <param name="entity">The entity to add</param>
        void Insert(TEntity entity);

        /// <summary>
        /// Updates the given entity
        /// </summary>
        /// <param name="entityToUpdate">The entity to update</param>
        void Update(TEntity entityToUpdate);

        /// <summary>
        /// Deletes the given entity
        /// </summary>
        /// <param name="entityToDelete">The entity to delete</param>
        void Delete(TEntity entityToDelete);

        /// <summary>
        /// Deletes the entity with the specified id
        /// </summary>
        /// <param name="id">Entity Id</param>
        void Delete(object id);
    }
}
