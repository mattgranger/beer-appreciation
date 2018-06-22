namespace BeerAppreciation.Data.Tests.Mocks
{
    using EF.Domain;

    /// <summary>
    /// A simple entity (no child entities) used for unit testing
    /// </summary>
    public class SimpleEntity : BaseEntityWithState<int>
    {
        /// <summary>
        /// Gets or sets some property.
        /// </summary>
        public string SomeProperty { get; set; }
    }
}
