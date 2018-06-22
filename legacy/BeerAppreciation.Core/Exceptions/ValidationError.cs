namespace BeerAppreciation.Core.Exceptions
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a validation error that occurred within the application layer
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Gets or sets the key of the field that has the error.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the errors associate with the field.
        /// </summary>
        public IList<string> Errors { get; set; }
    }
}
