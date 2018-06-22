namespace BeerAppreciation.Core.Authorisation
{
    /// <summary>
    /// The OAuth scopes for INX Application Services
    /// </summary>
    public static class OAuthScope
    {
        /// <summary>
        /// The scope claim type
        /// </summary>
        public const string ScopeClaimType = "urn:oauth:scope";

        /// <summary>
        /// The administration scope
        /// </summary>
        public const string Analytics = "analytics";

        /// <summary>
        /// The administration scope
        /// </summary>
        public const string Administration = "administration";

        /// <summary>
        /// The Common scope
        /// </summary>
        public const string Common = "common";
    }
}
