using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace load_board_api.Persistence
{
    /// <summary>
    /// Represents a generic repository for a specific entity type
    /// </summary>
    /// <typeparam name="TEntity">The type of entity stored in the repository</typeparam>
    public class Repo<TEntity> : IRepo<TEntity> where TEntity : class
    {
        internal LoadBoardDbContext context;
        internal DbSet<TEntity> dbSet;

        public Repo(LoadBoardDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Determines if an entity with the given id exists
        /// </summary>
        /// <param name="id">Entity Id</param>
        /// <returns>Boolean value indicating whether the entity exists</returns>
        public bool Exists(object id)
        {
            return this.dbSet.Find(id) != null;
        }

        /// <summary>
        /// Gets matching entities
        /// </summary>
        /// <param name="filter">Filter condition</param>
        /// <param name="skip">The number of entities to skip</param>
        /// <param name="num">The number of entities to return</param>
        /// <param name="orderBy">Column to order results by</param>
        /// <param name="includeProperties">Properties to eager-load</param>
        /// <returns>Matching entities</returns>
        public IEnumerable<TEntity> Get(
            System.Linq.Expressions.Expression<Func<TEntity, bool>> filter,
            int skip,
            int num,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string includeProperties
        )
        {
            IQueryable<TEntity> query = this.dbSet;

            if (filter != null)
            {
                query = query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);

                if (skip > -1)
                {
                    query = query.Skip(skip);
                }

                if (num > -1)
                {
                    query = query.Take(num);
                }
            }

            

            return query.ToList();
        }

        /// <summary>
        /// Gets the entity with the specified id
        /// </summary>
        /// <param name="id">Entity Id</param>
        /// <returns>Entity</returns>
        public TEntity Get(object id)
        {
            return this.dbSet.Find(id);
        }

        /// <summary>
        /// Adds the given entity
        /// </summary>
        /// <param name="entity">The entity to add</param>
        public void Insert(TEntity entity)
        {
            this.dbSet.Add(entity);
        }

        /// <summary>
        /// Updates the given entity
        /// </summary>
        /// <param name="entityToUpdate">The entity to update</param>
        public void Update(TEntity entityToUpdate)
        {
            this.context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        /// <summary>
        /// Deletes the given entity
        /// </summary>
        /// <param name="entityToDelete">The entity to delete</param>
        public void Delete(TEntity entityToDelete)
        {
            if (this.context.Entry(entityToDelete).State == EntityState.Detached)
            {
                this.dbSet.Attach(entityToDelete);
            }
            this.dbSet.Remove(entityToDelete);
        }

        /// <summary>
        /// Deletes the entity with the specified id
        /// </summary>
        /// <param name="id">Entity Id</param>
        public void Delete(object id)
        {
            TEntity entityToDelete = this.dbSet.Find(id);
            Delete(entityToDelete);
        }
    }
}