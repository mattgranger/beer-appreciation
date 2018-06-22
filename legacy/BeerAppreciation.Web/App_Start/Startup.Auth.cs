using System;
using System.Configuration;
using BeerAppreciation.Core.Authorisation;
using BeerAppreciation.Data.Repositories.Context;
using BeerAppreciation.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using BeerAppreciation.Web.Providers;

namespace BeerAppreciation.Web
{
    using System.Web;

    public partial class Startup
    {
        // Enable the application to use OAuthAuthorization. You can then secure your Web APIs
        static Startup()
        {
            PublicClientId = "web";
        }

        /// <summary>
        /// Gets the o authentication options.
        /// </summary>
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        /// <summary>
        /// Gets the public client identifier.
        /// </summary>
        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(DatabaseContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, Appreciator>(
                        validateInterval: TimeSpan.FromMinutes(20),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager)),
                    OnApplyRedirect = this.OnApplyRedirect
                }
            });

            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enable the application to use bearer tokens to authenticate users
            //app.UseOAuthBearerTokens(OAuthOptions);

            ConfigureOAuth(app);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }

        /// <summary>
        /// Called when [apply redirect].
        /// </summary>
        /// <param name="ctx">The CTX.</param>
        private void OnApplyRedirect(CookieApplyRedirectContext ctx)
        {
            EnsureSsl(ctx);

            if (!ctx.Request.Headers.ContainsKey(ApplicationContants.OAuthClientHeaderKey) || !IsApiRequest(ctx.Request))
            {
                ctx.Response.Redirect(ctx.RedirectUri);
            }
        }

        private static void EnsureSsl(CookieApplyRedirectContext ctx)
        {
            if (ctx.RedirectUri.IndexOf("http:", StringComparison.OrdinalIgnoreCase) > -1)
            {
                Uri absoluteUri;
                if (Uri.TryCreate(ctx.RedirectUri, UriKind.Absolute, out absoluteUri))
                {
                    var path = PathString.FromUriComponent(absoluteUri);
                    if (path == ctx.OwinContext.Request.PathBase + ctx.Options.LoginPath)
                    {
                        ctx.RedirectUri = ctx.RedirectUri.Replace("http:", "https:");
                        ctx.Response.Redirect(ctx.RedirectUri);
                    }
                }
            }
        }

        private static bool IsApiRequest(IOwinRequest request)
        {
            string apiPath = VirtualPathUtility.ToAbsolute("~/api/");
            return request.Uri.LocalPath.StartsWith(apiPath);
        }

        /// <summary>
        /// Configures authentication to use OAuth
        /// </summary>
        /// <param name="appBuilder">The application builder.</param>
        private static void ConfigureOAuth(IAppBuilder appBuilder)
        {
            string encryptionKey = ConfigurationManager.AppSettings["Encryption.AES.Key"];
            int accessTokenExpiryInSeconds = int.Parse(ConfigurationManager.AppSettings["OAuth.AccessToken.ExpiryInSeconds"]);

            if (string.IsNullOrEmpty(encryptionKey))
            {
                throw new InvalidOperationException("An AES encryption key 'Encryption.AES.Key' must be specfied in the app settings section of the config file");
            }

            var applicationUserManager = ApplicationUserManager.CreateDefault();
            CellarDoorAuthorisationServerProvider provider = new CellarDoorAuthorisationServerProvider(
                applicationUserManager);

            OAuthAuthorizationServerOptions authorisationServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(accessTokenExpiryInSeconds),
                Provider = provider,
                RefreshTokenProvider = new RefreshTokenProvider(),
                AccessTokenFormat = new SecureTokenFormatter(encryptionKey)
            };

            appBuilder.UseOAuthBearerTokens(authorisationServerOptions);
            appBuilder.UseOAuthAuthorizationServer(authorisationServerOptions);
        }

    }
}
