using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BeerAppreciation.Core.Authorisation;
using log4net;
using Microsoft.Owin.Security.OAuth;

namespace BeerAppreciation.Web.Providers
{
    using Domain;

    public class CellarDoorAuthorisationServerProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// The user manager
        /// </summary>
        private readonly ApplicationUserManager userManager;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILog logger = LogManager.GetLogger("INX");

        /// <summary>
        /// Initializes a new instance of the <see cref="CellarDoorAuthorisationServerProvider" /> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        public CellarDoorAuthorisationServerProvider(ApplicationUserManager userManager)
        {
            this.userManager = userManager;
        }

        /// <summary>
        /// Validates the oAuth Client credentials
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A Task</returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                // TODO: In the future we could introduce a mechanism in that a client has to provide a password (client secret), for now just having the client Id in the 
                // TODO: authorization header is enough
                return;
            }

            context.Validated();
        }

        /// <summary>
        /// Authenticates the resource owner username and password and returns an oAuth token
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A Task</returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            try
            {
                ClaimsIdentity claimsIdentity = await this.Authorise(context.UserName, context.Password, context.Options.AuthenticationType, context.Scope);

                if (claimsIdentity == null)
                {
                    return;
                }

                context.Validated(claimsIdentity);

                this.LogBearerTokenClaims(context.UserName, claimsIdentity);
            }
            catch (Exception ex)
            {
                this.logger.Error("Error occurred validating user with Active Directory", ex);

                throw;
            }
        }

        /// <summary>
        /// Authorises the specified user name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="authenticationType">Type of the authentication.</param>
        /// <param name="scope">The scope.</param>
        /// <returns></returns>
        private async Task<ClaimsIdentity> Authorise(string userName, string password, string authenticationType, IList<string> scope)
        {
            var validUser = await this.userManager.FindAsync(userName, password);
            if (validUser == null)
            {
                return null;
            }

            return CreateUserClaims(validUser, authenticationType, scope);
        }

        /// <summary>
        /// Creates the user claims.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="authenticationType">Type of the authentication.</param>
        /// <param name="scopes">The scope requested by the client (i.e. what the token will be able to access, e.g. InViron Service).</param>
        /// <returns>
        /// The user claims
        /// </returns>
        private static ClaimsIdentity CreateUserClaims(Appreciator user, string authenticationType, IList<string> scopes)
        {
            IList<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Roles", GetUserRoleString(user))
            };

            AddScopesToClaims(claims, scopes);

            return new ClaimsIdentity(claims, authenticationType);
        }

        private static string GetUserRoleString(Appreciator user)
        {
            if (user == null || !user.Roles.Any())
            {
                return string.Empty;
            }

            return string.Join("|", user.Roles.Select(r => r.RoleId));
        }

        /// <summary>
        /// Adds the required scopes as claims
        /// </summary>
        /// <param name="claims">The claims list that the scope will be added to</param>
        /// <param name="scopes">The scope requested by the client (i.e. what the token will be able to access, e.g. InViron Service).</param>
        private static void AddScopesToClaims(IList<Claim> claims, IList<string> scopes)
        {
            // This InViron authorisation provider can only provide scope to the InVirionService, anything other requested scopes will be ignored
            if (scopes.Any(s => s.Equals(OAuthScope.Administration, StringComparison.OrdinalIgnoreCase)))
            {
                claims.Add(new Claim(OAuthScope.ScopeClaimType, OAuthScope.Administration));
            }
        }


        /// <summary>
        /// Gets a more verbose name of the claim type and not the full namespace
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <returns>
        /// A verbose name for the claim type
        /// </returns>
        private static string GetClaimTypeVerboseName(Claim claim)
        {
            int position = claim.Type.LastIndexOf("/", StringComparison.OrdinalIgnoreCase);

            if (position >= 0)
            {
                return claim.Type.Substring(position + 1);
            }

            return claim.Type;
        }

        /// <summary>
        /// Logs the bearer token claims.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="claimsIdentity">The claims principal.</param>
        private void LogBearerTokenClaims(string userName, ClaimsIdentity claimsIdentity)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"Created a bearer token containing the following claims for user '{userName}':");

            foreach (Claim claim in claimsIdentity.Claims)
            {
                builder.AppendLine($"{GetClaimTypeVerboseName(claim)}: {claim.Value}");
            }

            this.logger.Info(builder.ToString());
        }
    }
}