namespace BeerAppreciation.Web.Controllers
{
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Collections.Generic;
    using System.Net.Http;
    using Data.Repositories.Context;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;

    [Authorize]
    public class SecurityController : ApiController
    {
        private readonly ApplicationUserManager userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public SecurityController()
        {
            this.userManager = HttpContext.Current.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            this.roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new DatabaseContext()));
        }

        [HttpGet]
        [Route("api/security/{userId}/roles")]
        public IList<string> GetUserRoles(string userId)
        {
            var userRoles = this.userManager.GetRolesAsync(userId).Result;
            return userRoles;
        }

        [HttpGet]
        [Route("api/security/roles")]
        public IList<IdentityRole> GetRoles()
        {
            return this.roleManager.Roles.ToList();
        }
    }
}
