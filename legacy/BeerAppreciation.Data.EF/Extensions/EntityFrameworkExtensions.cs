using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using BeerAppreciation.Data.EF.Domain;

namespace BeerAppreciation.Data.EF.Extensions
{
    /// <summary>
    /// Extensions for entity framework related functionality
    /// </summary>
    public static class EntityFrameworkExtensions
    {
        #region Public Methods

        /// <summary>
        /// Gets the primary key of the first entity that was inserted for a given entitySet during an entity framework SaveChanges
        /// </summary>
        /// <typeparam name="T">The type of the entity key</typeparam>
        /// <param name="entities">The entities.</param>
        /// <param name="entitySet">The entity set.</param>
        /// <returns>
        /// THe primary key of the matching entity
        /// </returns>
        public static T GetInsertedEntityKey<T>(this IList<ObjectStateEntry> entities, string entitySet)
        {
            // Locate the first entity for the specified entity set
            ObjectStateEntry entity = entities.FirstOrDefault(e => e.EntitySet.Name == entitySet);

            if (entity != null && entity.EntityKey != null && entity.EntityKey.EntityKeyValues.Any())
            {
                // Get the first value, need a separate extension method if we want to cater for composite primary keys
                object value = entity.EntityKey.EntityKeyValues.FirstOrDefault().Value;

                // Convert to the specified type and return
                return (T)Convert.ChangeType(value, typeof(T));
            }

            return default(T);
        }

        /// <summary>
        /// Finds an entity in the context
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="dbContext">The database context.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The matching DBEntityEntry
        /// </returns>
        public static DbEntityEntry FindEntity<TKey>(this DbContext dbContext, TKey id) where TKey : struct
        {
            return dbContext.ChangeTracker.Entries<IEntityWithState<TKey>>()
                .FirstOrDefault(entry => entry.Entity.Id.Equals(id));
        }

        #endregion
    }
}
