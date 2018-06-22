namespace BeerAppreciation.Data.Tests.Mocks
{
    using System.Collections.Generic;
    using EF.Domain;

    /// <summary>
    /// An aggregate root entity (has child entities) used for unit testing
    /// </summary>
    public class AggregateRootEntity : BaseEntityWithState<int>
    {
        /// <summary>
        /// Gets or sets the simple entity.
        /// </summary>
        public SimpleEntity SimpleEntity { get; set; }

        /// <summary>
        /// Gets or sets the other simple entities.
        /// </summary>
        public IList<AnotherSimpleEntity> OtherSimpleEntities { get; set; }

        /// <summary>
        /// Gets or sets some property.
        /// </summary>
        public string SomeOtherProperty { get; set; }
    }
}
