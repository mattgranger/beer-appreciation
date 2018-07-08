namespace BeerAppreciation.Services.Identity.API.Controllers
{
    using System.Threading.Tasks;
    using API;
    using IdentityServer4.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Models;
    using Services;

    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService interaction;
        private readonly IOptionsSnapshot<AppSettings> settings;
        private readonly IRedirectService redirectSvc;

        public HomeController(IIdentityServerInteractionService interaction, IOptionsSnapshot<AppSettings> settings,IRedirectService redirectSvc)
        {
            this.interaction = interaction;
            this.settings = settings;
            this.redirectSvc = redirectSvc;
        }

        public IActionResult Index(string returnUrl)
        {
            return this.View();
        }

        public IActionResult ReturnToOriginalApplication(string returnUrl)
        {
            if (returnUrl != null)
                return this.Redirect(this.redirectSvc.ExtractRedirectUriFromReturnUrl(returnUrl));
            else
                return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await this.interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;
            }

            return this.View("Error", vm);
        }
    }
}