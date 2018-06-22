using System;
using System.Configuration;
using Microsoft.Owin.Security.Infrastructure;

namespace BeerAppreciation.Web.Providers
{
    /// <summary>
    /// Provider responsible for generating oAuth refresh tokens
    /// </summary>
    public class RefreshTokenProvider : AuthenticationTokenProvider
    {
        /// <summary>
        /// Creates a refresh token, this method is called by the OWIN framework when an oAuth token request is received
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Create(AuthenticationTokenCreateContext context)
        {
            int expirySeconds = int.Parse(ConfigurationManager.AppSettings["OAuth.RefreshToken.ExpiryInSeconds"]);
            context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddSeconds(expirySeconds));

            context.SetToken(context.SerializeTicket());
        }

        /// <summary>
        /// Method invoked by the OWIN framework when a Refresh Token is sent by a client to get a new Access Token
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
        }
    }
}