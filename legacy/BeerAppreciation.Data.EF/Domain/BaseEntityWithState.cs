namespace BeerAppreciation.Data.EF.Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The base class containing for an IEntityWithState
    /// </summary>
    /// <typeparam name="TKey">The type of primary key for this entity.</typeparam>
    public class BaseEntityWithState<TKey> : IEntityWithState<TKey>
    {
        /// <summary>
        /// Gets or sets the identifier representing the entity's primary key
        /// </summary>
        [Key]
        public TKey Id { get; set; }

        /// <summary>
        /// Gets or sets the state of the entity.
        /// </summary>
        [NotMapped]
        public State State { get; set; }
    }
}
