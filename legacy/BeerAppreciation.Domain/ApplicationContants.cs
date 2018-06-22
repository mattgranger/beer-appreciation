using System;

namespace BeerAppreciation.Domain
{
    /// <summary>
    /// Defines application constants
    /// </summary>
    public static class ApplicationContants
    {
        /// <summary>
        /// The o authentication header key
        /// </summary>
        public static readonly string OAuthClientHeaderKey = "OAuthClient";

        /// <summary>
        /// The authentication client identifier for the mobile app
        /// </summary>
        public static readonly Guid OAuthClientId = new Guid("{EE310BAA-688B-4845-9BF4-A2C33DD6C47C}");
    }
}
