namespace BeerAppreciation.Data.EF.Domain
{
    /// <summary>
    /// An enumeration of the various states an entity can be in
    /// </summary>
    public enum State
    {
        /// <summary>
        /// The default entry
        /// </summary>
        NotSet = 0,

        /// <summary>
        /// The entity has no changes
        /// </summary>
        Unchanged = 1,

        /// <summary>
        /// An entity has been added to context and will be persisted to database with next call to SaveChanges
        /// </summary>
        Added = 2,

        /// <summary>
        /// An entity has been modified will be persisted to database with next call to SaveChanges
        /// </summary>
        Modified = 3,

        /// <summary>
        /// An entity has been deleted and will be removed from database with next call to SaveChanges
        /// </summary>
        Deleted = 4
    }
}
