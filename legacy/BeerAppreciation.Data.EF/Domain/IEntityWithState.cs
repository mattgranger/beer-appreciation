namespace BeerAppreciation.Data.EF.Domain
{
    /// <summary>
    /// Generic interface defining properties related to the primary key of an entity
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key representing this entity.</typeparam>
    public interface IEntityWithState<TKey> : IEntityWithState
    {
        /// <summary>
        /// Gets or sets the primary key of the entity
        /// </summary>
        TKey Id { get; set; }
    }

    /// <summary>
    /// Interface defining a property used to track the state of an object
    /// </summary>
    public interface IEntityWithState
    {
        /// <summary>
        /// Gets or sets the state of the entity.
        /// </summary>
        State State { get; set; }
    }
}
